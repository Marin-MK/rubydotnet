using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace rubydotnet;

public class Tokenizer
{
    private static List<string> Keywords = new List<string>()
    {
        "class", "def", "if", "true", "false", "else", "end", "begin", "end", "rescue", "ensure",
        "return", "next", "break", "yield", "alias", "elsif", "case", "when", "module", "not", "and", "or",
        "redo", "retry", "for", "undef", "unless", "super", "then", "while", "defined?", "self", "raise"
    };

    private static List<(string RegExp, string TokenName)> Patterns = new List<(string RegExp, string TokenName)>()
    {
        (@"#.*?($|[\r\n])", "comment"),
        (@"/[^/]*/[eimnosux]*", "regex"),
        (@"'.*?(?<!\\)'", "string"),
        (@"[A-Z][A-Za-z0-9_:]*", "constant"),
        (@"[a-z_][A-Za-z0-9_:?]*", "variable_or_method"),
        (@"@[A-Za-z0-9_:?]+", "instance_variable"),
        (@"@@[A-Za-z0-9_:?]+", "class_variable"),
        (@"\$[a-zA-Z0-9_]+", "global_variable"),
        (@"\$[!@\.=\*\$/\?~&`'+]", "global_variable"),
        (@":[A-Za-z0-9_:?'""]+", "symbol"),
        (@"\(\)", "empty_method"),
        (@"\(", "parenthesis_open"),
        (@"\)", "parenthesis_close"),
        (@"(!|&&|\|\|)", "logical_operator"),
        (@"(<<|>>)={0,1}", "bitwise_operator"),
        (@"=", "assignment"),
        (@"(!=|<|<=|>|>=|==)", "relational_operator"),
        (@"[~\*+\-/%\^\|&]={0,1}", "arithmetic_operator"),
        (@"\.{2,3}", "range"),
        (@"\.", "object_access"),
        (@";", "line_end"),
        (@"[\?:]", "ternary_operator"),
        (@"\[\]", "array_initialization"),
        (@"\{\}", "hash_initialization"),
        (@"[\[\]]", "array_access"),
        (@"[{}]", "block"),
        (@",", "argument_list")
    };

    private static Dictionary<string, Regex> RegexCache = new Dictionary<string, Regex>();

    private int Caret;
    private string String;

    public static List<Token> Tokenize(string String, bool FilterComments = false, bool UseDefaultToken = false)
    {
        Tokenizer tokenizer = new Tokenizer(String);
        return tokenizer.Run(FilterComments, UseDefaultToken);
    }

    private Tokenizer(string String)
    {
        this.Caret = 0;
        this.String = String;
    }

    private List<Token> Run(bool FilterComments = false, bool UseDefaultToken = false)
    {
        List<Token> Tokens = new List<Token>();

        Token LastToken = null;

        while (HasToken())
        {
            Token t = GetNextToken(UseDefaultToken);
            if (FilterComments && t.Type == "comment" || t.Type == "multiline_comment") continue;
            if (LastToken != null)
            {
                if (t.Type == "variable_or_method" && LastToken.Type == "def") t.Type = "method_definition";
                else if (t.Type == "constant" && LastToken.Type == "class") t.Type = "class_definition";
                else if (t.Type == "parenthesis_open" && LastToken.Type == "variable_or_method") LastToken.Type = "method";
                else if (t.Type == "empty_method" && LastToken.Type == "variable_or_method") LastToken.Type = "method";
            }
            Tokens.Add(t);
            LastToken = t;
        }

        return Tokens;
    }

    private bool HasToken()
    {
        return Caret < String.Length;
    }

    int StringLevel = 0;
    int InterpolationLevel = 0;
    bool NextQuoteIsOpener = true;

    private Token GetNextToken(bool UseDefaultToken = false)
    {
        bool StringStartOrEnd = GetRegex("^\"").Match(String, Caret, String.Length - Caret).Success;
        if (StringLevel > InterpolationLevel && GetRegex("^#{").Match(String, Caret, String.Length - Caret).Success)
        {
            Caret += 2;
            InterpolationLevel++;
            NextQuoteIsOpener = true;
            return new Token("open_string_interpolation", "#{", Caret - 2, 2);
        }
        else if (InterpolationLevel > 0 && GetRegex("^}").Match(String, Caret, String.Length - Caret).Success)
        {
            Caret += 1;
            InterpolationLevel--;
            NextQuoteIsOpener = false;
            return new Token("close_string_interpolation", "}", Caret - 1, 1);
        }
        else if (StringLevel > InterpolationLevel || StringStartOrEnd)
        {
            int startidx = Caret + (NextQuoteIsOpener ? 1 : 0);
            int idx = -1;
            if (StringStartOrEnd)
            {
                if (NextQuoteIsOpener)
                {
                    StringLevel++;
                }
                else
                {
                    Caret++;
                    StringLevel--;
                    NextQuoteIsOpener = true;
                    return new Token("string", "\"", Caret - 1, 1);
                }
            }
            for (int i = startidx; i < String.Length; i++)
            {
                if (String[i] == '"' && (i == 0 || String[i - 1] != '\\'))
                {
                    StringLevel--;
                    idx = i + 1;
                    break;
                }
                else if (String[i] == '{' && String[i - 1] == '#')
                {
                    // Escape on \#{, but do not escape on \\#{
                    if (i - 3 >= 0)
                    {
                        if (String[i - 2] == '\\' && String[i - 3] != '\\') continue;
                    }
                    else if (i - 2 >= 0)
                    {
                        if (String[i - 2] == '\\') continue;
                    }
                    idx = i - 1;
                    break;
                }
            }
            if (idx == -1) idx = String.Length;
            int StartPos = Caret;
            int Length = idx - Caret;
            Caret += Length;
            NextQuoteIsOpener = true;
            return new Token("string", String.Substring(StartPos, Length), StartPos, Length);
        }
        if (String[Caret] == '\r' || String[Caret] == '\n' || String[Caret] == ' ')
        {
            Caret++;
            return GetNextToken();
        }
        if (GetRegex("^=begin").Match(String, Caret, String.Length - Caret).Success)
        {
            Match endmatch = GetRegex("[\r\n]+=end").Match(String, Caret, String.Length - Caret);
            int idx = 0;
            if (!endmatch.Success) idx = String.Length;
            else idx = endmatch.Index + endmatch.Length;
            int StartPos = Caret;
            int Length = idx - StartPos;
            Caret += Length;
            return new Token("multiline_comment", String.Substring(StartPos, Length), StartPos, Length);
        }
        Match allhexmatch = GetRegex(@"^0[xX][a-zA-Z0-9_]+").Match(String, Caret, String.Length - Caret);
        if (allhexmatch.Success)
        {
            string hexnum = allhexmatch.Groups[0].Value;
            int StartPos = Caret;
            Caret += hexnum.Length;
            bool invalidhex = GetRegex(@"[g-zG-Z]").Match(hexnum, 2, hexnum.Length - 2).Success;
            if (!hexnum.Contains("__") && hexnum[2] != '_' && !hexnum.EndsWith('_') && !invalidhex)
            {
                // Filter out hex numbers with two consecutive underscores, and hex numbers
                // beginning or ending with an underscore.
                return new Token("hex", hexnum, StartPos, hexnum.Length);
            }
            return new Token("invalid_hex", hexnum, StartPos, hexnum.Length);
        }
        Match nummatch = GetRegex(@"^\d+[\d+_]*").Match(String, Caret, String.Length - Caret);
        if (nummatch.Success)
        {
            string num = nummatch.Groups[0].Value;
            int StartPos = Caret;
            Caret += num.Length;
            if (!num.Contains("__") && !num.EndsWith('_'))
            {
                // Filter out numbers with two consecutive underscores, and
                // numbers beginning or ending with an underscore.
                return new Token("number", num, StartPos, num.Length);
            }
            return new Token("invalid_number", num, StartPos, num.Length);
        }
        for (int i = 0; i < Keywords.Count; i++)
        {
            string pattern = "^" + Keywords[i] + @"($|[\r\n\. ])";
            Match m = GetRegex(pattern).Match(String, Caret, String.Length - Caret);
            if (!m.Success) continue;
            int StartPos = Caret;
            Caret += Keywords[i].Length;
            return new Token(Keywords[i], Keywords[i], StartPos, Keywords[i].Length);
        }
        for (int i = 0; i < Patterns.Count; i++)
        {
            (string RegExp, string TokenName) = Patterns[i];
            Match match = GetRegex("^" + RegExp).Match(String, Caret, String.Length - Caret);
            if (!match.Success) continue;
            int StartPos = Caret;
            Caret += match.Length;
            string Value = match.Groups[0].Value;
            return new Token(TokenName, Value, StartPos, match.Length);
        }
        if (UseDefaultToken)
        {
            int idx = -1;
            for (int i = Caret; i < String.Length; i++)
            {
                if (String[i] == '\n' || String[i] == '\r' || String[i] == ' ')
                {
                    idx = i;
                    break;
                }
            }
            if (idx == -1) idx = String.Length;
            int StartPos = Caret;
            int Length = idx - StartPos;
            Caret += Length;
            return new Token("unknown", String.Substring(StartPos, Length), StartPos, Length);
        }
        throw new Exception($"Unknown token: {String[Caret]}");
    }

    private static Regex GetRegex(string Pattern)
    {
        if (RegexCache.ContainsKey(Pattern)) return RegexCache[Pattern];
        RegexCache.Add(Pattern, new Regex(Pattern));
        return RegexCache[Pattern];
    }
}
