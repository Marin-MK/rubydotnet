using System;
using System.Collections.Generic;
using System.Text;

namespace rubydotnet
{
    public static partial class Ruby
    {
        public class MatchData : Object
        {
            public new static string KlassName = "MatchData";
            public new static Class Class { get => (Class) GetKlass(KlassName); }

            public MatchData(IntPtr Pointer) : base(Pointer, true) { }

            public int Length
            {
                get => Funcall("length").Convert<Integer>();
            }

            public Integer Begin(Integer n)
            {
                return Funcall("begin", n).Convert<Integer>();
            }
            public Integer Begin(String Name)
            {
                return Funcall("begin", Name).Convert<Integer>();
            }
            public Integer Begin(Symbol Name)
            {
                return Funcall("begin", Name).Convert<Integer>();
            }
            public Integer End(Integer n)
            {
                return Funcall("end", n).Convert<Integer>();
            }
            public Integer End(String Name)
            {
                return Funcall("end", Name).Convert<Integer>();
            }
            public Integer End(Symbol Name)
            {
                return Funcall("end", Name).Convert<Integer>();
            }
            public Array Captures()
            {
                return Funcall("captures").Convert<Array>();
            }
            public Hash NamedCaptures()
            {
                return Funcall("named_captures").Convert<Hash>();
            }
            public Array Names()
            {
                return Funcall("names").Convert<Array>();
            }
            public Array Offset(Integer n)
            {
                return Funcall("offset", n).Convert<Array>();
            }
            public Array Offset(String Name)
            {
                return Funcall("offset", Name).Convert<Array>();
            }
            public Array Offset(Symbol Name)
            {
                return Funcall("offset", Name).Convert<Array>();
            }
            public String PreMatch()
            {
                return Funcall("pre_match").Convert<String>();
            }
            public String PostMatch()
            {
                return Funcall("post_match").Convert<String>();
            }
            public Regexp Regexp()
            {
                return Funcall("regexp").Convert<Regexp>();
            }
            public String String()
            {
                return Funcall("string").Convert<String>();
            }

            public Object this[Integer Index]
            {
                get => Funcall("[]", Index);
            }
            public Object this[Integer Index, Integer Length]
            {
                get => Funcall("[]", Index, Length);
            }
            public Object this[Range r]
            {
                get => Funcall("[]", r);
            }
            public Object this[String Name]
            {
                get => Funcall("[]", Name);
            }
            public Object this[Symbol Name]
            {
                get => Funcall("[]", Name);
            }
        }
    }
}
