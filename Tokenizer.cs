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
        (@"#.*?[\r\n]", "comment"),
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
        string text = String.Substring(Caret);
        if (text.StartsWith("#{") && StringLevel > InterpolationLevel)
        {
            Caret += 2;
            InterpolationLevel++;
            NextQuoteIsOpener = true;
            return new Token("open_string_interpolation", "#{", Caret - 2, 2);
        }
        else if (text.StartsWith('}') && InterpolationLevel > 0)
        {
            Caret += 1;
            InterpolationLevel--;
            NextQuoteIsOpener = false;
            return new Token("close_string_interpolation", "}", Caret - 1, 1);
        }
        else if (text.StartsWith('"') || StringLevel > InterpolationLevel)
        {
            int startidx = NextQuoteIsOpener ? 1 : 0;
            int idx = -1;
            int increase = -1;
            if (text.StartsWith('"'))
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
            for (int i = startidx; i < text.Length; i++)
            {
                if (text[i] == '"' && (i == 0 || text[i - 1] != '\\'))
                {
                    StringLevel--;
                    idx = i + 1;
                    increase = idx;
                    break;
                }
                else if (text[i] == '{' && text[i - 1] == '#')
                {
                    // Escape on \#{, but do not escape on \\#{
                    if (i - 3 >= 0)
                    {
                        if (text[i - 2] == '\\' && text[i - 3] != '\\') continue;
                    }
                    else if (i - 2 >= 0)
                    {
                        if (text[i - 2] == '\\') continue;
                    }
                    idx = i - 1;
                    increase = idx;
                    break;
                }
            }
            if (idx == -1)
            {
                idx = text.Length;
                increase = idx;
            }
            int StartPos = Caret;
            Caret += increase;
            NextQuoteIsOpener = true;
            return new Token("string", text.Substring(0, idx), StartPos, idx);
        }
        if (String[Caret] == '\r' || String[Caret] == '\n' || String[Caret] == ' ')
        {
            Caret++;
            return GetNextToken();
        }
        if (text.StartsWith("=begin"))
        {
            Match endmatch = Regex.Match(text, "[\r\n]+=end");
            int idx = 0;
            if (!endmatch.Success) idx = text.Length;
            else idx = endmatch.Index + endmatch.Length;
            int StartPos = Caret;
            Caret += idx;
            return new Token("multiline_comment", text.Substring(0, idx), StartPos, idx);
        }
        if (text.StartsWith("0x") || text.StartsWith("0X"))
        {
            Match m = Regex.Match(text, @"^0[xX][a-zA-Z0-9_]+");
            if (m.Success)
            {
                string hexnum = m.Groups[0].Value;
                int StartPos = Caret;
                Caret += hexnum.Length;
                if (!hexnum.Contains("__") && hexnum[2] != '_' && !hexnum.EndsWith('_') && !Regex.IsMatch(hexnum.Substring(2), @"[g-zG-Z]"))
                {
                    // Filter out hex numbers with two consecutive underscores, and hex numbers
                    // beginning or ending with an underscore.
                    return new Token("hex", hexnum, StartPos, hexnum.Length);
                }
                return new Token("invalid_hex", hexnum, StartPos, hexnum.Length);
            }
        }
        Match nummatch = Regex.Match(text, @"^\d+[\d+_]*");
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
            if (!text.StartsWith(Keywords[i])) continue;
            string pattern = "^" + Keywords[i] + @"[\r\n\. ]";
            Match m = Regex.Match(text, pattern);
            if (text != Keywords[i] && !m.Success) continue;
            int StartPos = Caret;
            Caret += Keywords[i].Length;
            return new Token(Keywords[i], Keywords[i], StartPos, Keywords[i].Length);
        }
        for (int i = 0; i < Patterns.Count; i++)
        {
            (string RegExp, string TokenName) = Patterns[i];
            Match match = Regex.Match(text, "^" + RegExp);
            if (!match.Success) continue;
            int StartPos = Caret;
            Caret += match.Length;
            string Value = match.Groups[0].Value;
            return new Token(TokenName, Value, StartPos, match.Length);
        }
        if (UseDefaultToken)
        {
            int idx = -1;
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == '\n' || text[i] == '\r' || text[i] == ' ')
                {
                    idx = i;
                    break;
                }
            }
            if (idx == -1) idx = text.Length;
            int StartPos = Caret;
            Caret += idx;
            return new Token("unknown", text.Substring(0, idx), StartPos, idx);
        }
        throw new Exception($"Unknown token: {text}");
    }
}
