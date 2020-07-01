using System;
using System.Collections.Generic;

namespace rubydotnet
{
    public static partial class Ruby
    {
        public class String : Object
        {
            public new static string KlassName = "String";
            public new static Class Class { get { return (Class) GetKlass(KlassName); } }

            public String(IntPtr Pointer) : base(Pointer, true) { }
            public String(IntPtr Pointer, bool Validate = true) : base(Pointer, Validate) { }

            public String(string Value) : base(STR2NUM(Value), false) { }

            public int ToInt32(int Base = 10)
            {
                return Funcall("to_i", (Integer) Base).Convert<Integer>();
            }
            public override string ToString()
            {
                return NUM2STR(this.Pointer);
            }
            public Symbol ToSymbol()
            {
                return new Symbol(ToString());
            }
            public override string Inspect()
            {
                string String = ToString();
                return '"' + String.Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("\n", "\\n").Replace("\r", "\\r") + '"';
            }

            public int Length
            {
                get => NUM2INT(rb_str_strlen(this.Pointer));
            }
            public bool Empty
            {
                get => Length == 0;
            }

            public Object this[Integer Index]
            {
                get => Funcall("[]", Index);
                set => Funcall("[]=", Index, value);
            }
            public Object this[Integer Index, Integer Length]
            {
                get => this[Index.ToInt32(), Length.ToInt32()];
                set => this[Index.ToInt32(), Length.ToInt32()] = value;
            }
            public Object this[Range range]
            {
                get => Funcall("[]", range);
                set => Funcall("[]=", range, value);
            }
            public Object this[String MatchStr]
            {
                get => Funcall("[]", MatchStr);
                set => Funcall("[]=", MatchStr, value);
            }
            public Object this[Regexp Regexp]
            {
                get => Funcall("[]", Regexp);
                set => Funcall("[]=", Regexp, value);
            }
            public Object this[Regexp Regexp, Integer Capture]
            {
                get => Funcall("[]", Regexp, Capture);
                set => Funcall("[]=", Regexp, Capture, value);
            }
            public Object this[Regexp Regexp, String Capture]
            {
                get => Funcall("[]", Regexp, Capture);
                set => Funcall("[]=", Regexp, Capture, value);
            }

            public String Capitalize()
            {
                return Funcall("capitalize").Convert<String>();
            }
            public String Chomp()
            {
                return Funcall("chomp").Convert<String>();
            }
            public String Chomp(String Separator)
            {
                return Funcall("chomp", Separator).Convert<String>();
            }
            public String Chop()
            {
                return Funcall("chop").Convert<String>();
            }
            public String Clear()
            {
                return Funcall("clear").Convert<String>();
            }
            public String Concat(params Object[] Objects)
            {
                return Funcall("concat", Objects).Convert<String>();
            }
            public String Delete(params String[] OtherStr)
            {
                return Funcall("delete", OtherStr).Convert<String>();
            }
            public String DeletePrefix(String OtherStr)
            {
                return Funcall("delete_prefix", OtherStr).Convert<String>();
            }
            public String DeleteSuffix(String OtherStr)
            {
                return Funcall("delete_suffix", OtherStr).Convert<String>();
            }
            public String Downcase()
            {
                return Funcall("downcase").Convert<String>();
            }
            public bool EndWith(params String[] Suffixes)
            {
                return Funcall("end_with?", Suffixes) == True;
            }
            public String Gsub(Regexp Regexp, String Replacement)
            {
                return Funcall("gsub", Regexp, Replacement).Convert<String>();
            }
            public String Gsub(Regexp Regexp, Hash Replacement)
            {
                return Funcall("gsub", Regexp, Replacement).Convert<String>();
            }
            public bool Include(String OtherStr)
            {
                return Funcall("include?", OtherStr) == True;
            }
            public Object Index(String Substring)
            {
                return Funcall("index", Substring);
            }
            public Object Index(String Substring, Integer Offset)
            {
                return Funcall("index", Substring, Offset);
            }
            public Object Index(Regexp Regexp)
            {
                return Funcall("index", Regexp);
            }
            public Object Index(Regexp Regexp, Integer Offset)
            {
                return Funcall("index", Regexp, Offset);
            }
            public String Insert(Integer Index, String OtherStr)
            {
                return Funcall("insert", Index, OtherStr).Convert<String>();
            }
            public String LJust(Integer Integer, String PadStr)
            {
                return Funcall("ljust", Integer, PadStr).Convert<String>();
            }
            public String LJust(Integer Integer)
            {
                return Funcall("ljust", Integer).Convert<String>();
            }
            public String Replace(String OtherStr)
            {
                return Funcall("replace", OtherStr).Convert<String>();
            }
            public String LStrip()
            {
                return Funcall("lstrip").Convert<String>();
            }
            public Object Match(Regexp Regexp)
            {
                return Funcall("match", Regexp);
            }
            public Object Match(Regexp Regexp, Integer Pos)
            {
                return Funcall("match", Regexp, Pos);
            }
            public Object Match(String Pattern)
            {
                return Funcall("match", Pattern);
            }
            public Object Match(String Pattern, Integer Pos)
            {
                return Funcall("match", Pattern, Pos);
            }
            public bool IsMatch(Regexp Regexp)
            {
                return Funcall("match?", Regexp) == True;
            }
            public bool IsMatch(Regexp Regexp, Integer Pos)
            {
                return Funcall("match?", Regexp, Pos) == True;
            }
            public bool IsMatch(String Pattern)
            {
                return Funcall("match?", Pattern) == True;
            }
            public bool IsMatch(String Pattern, Integer Pos)
            {
                return Funcall("match?", Pattern, Pos) == True;
            }
            public String Prepend(params String[] Strings)
            {
                return Replace(Funcall("prepend", Strings).Convert<String>());
            }
            public String Reverse()
            {
                return Funcall("reverse").Convert<String>();
            }
            public Object RIndex(String Substring)
            {
                return Funcall("rindex", Substring);
            }
            public Object RIndex(String Substring, Integer Limit)
            {
                return Funcall("rindex", Substring, Limit);
            }
            public Object RIndex(Regexp Regexp)
            {
                return Funcall("rindex", Regexp);
            }
            public Object RIndex(Regexp Regexp, Integer Limit)
            {
                return Funcall("rindex", Regexp, Limit);
            }
            public String RJust(Integer Integer, String PadStr)
            {
                return Funcall("rjust", Integer, PadStr).Convert<String>();
            }
            public String RJust(Integer Integer)
            {
                return Funcall("rjust", Integer).Convert<String>();
            }
            public String RStrip()
            {
                return Funcall("rstrip").Convert<String>();
            }
            public Array Scan(Regexp Regexp)
            {
                return Funcall("scan", Regexp).Convert<Array>();
            }
            public Array Split()
            {
                return Funcall("split").Convert<Array>();
            }
            public Array Split(String Pattern)
            {
                return Funcall("split", Pattern).Convert<Array>();
            }
            public Array Split(String Pattern, Integer Limit)
            {
                return Funcall("split", Pattern, Limit).Convert<Array>();
            }
            public Array Split(Regexp Regexp)
            {
                return Funcall("split", Regexp).Convert<Array>();
            }
            public Array Split(Regexp Regexp, Integer Limit)
            {
                return Funcall("split", Regexp, Limit).Convert<Array>();
            }
            public bool StartWith(params String[] Prefixes)
            {
                return Funcall("start_with?", Prefixes) == True;
            }
            public String Strip()
            {
                return Funcall("strip").Convert<String>();
            }
            public String Sub(Regexp Regexp, String Replacement)
            {
                return Funcall("sub", Regexp, Replacement).Convert<String>();
            }
            public String Sub(Regexp Regexp, Hash Replacement)
            {
                return Funcall("sub", Regexp, Replacement).Convert<String>();
            }
            public String Swapcase()
            {
                return Funcall("swapcase").Convert<String>();
            }
            public Array Unpack(String Format)
            {
                return Funcall("unpack", Format).Convert<Array>();
            }
            public String Upcase()
            {
                return Funcall("upcase").Convert<String>();
            }

            public static bool operator ==(String One, String Two)
            {
                return One.ToString() == Two.ToString();
            }
            public static bool operator !=(String One, String Two)
            {
                return One.ToString() != Two.ToString();
            }

            public override bool Equals(object obj)
            {
                if (obj is String)
                {
                    return this == (String) obj;
                }
                else if (obj is string)
                {
                    return this == (String) obj;
                }
                return base.Equals(obj);
            }

            public override int GetHashCode()
            {
                return ToString().GetHashCode();
            }

            public static implicit operator string(String s) => s.ToString();
            public static implicit operator String(string s) => new String(s);
        }
    }
}
