using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace rubydotnet
{
    public static partial class Ruby
    {
        public class Regexp : Object
        {
            public new static string KlassName = "Regexp";
            public new static Class Class { get => (Class) GetKlass(KlassName); }

            public Regexp(IntPtr Pointer) : base(Pointer, true) { }

            public Regexp(string Pattern, bool iIgnoreCase = false, bool xIgnoreWhitespace = false, bool mMatchMultiline = false)
                : this(rb_reg_new_str(STR2NUM(Pattern), (iIgnoreCase ? 1 : 0) | (xIgnoreWhitespace ? 2 : 0) | (mMatchMultiline ? 4 : 0))) { }

            public override string ToString()
            {
                return Funcall("inspect").Convert<String>().ToString();
            }

            public static implicit operator Regexp(string Pattern) => new Regexp(Pattern);
            public static implicit operator Regexp((string Pattern, bool iIgnoreCase) r) => new Regexp(r.Pattern, r.iIgnoreCase);
            public static implicit operator Regexp((string Pattern, bool iIgnoreCase, bool xIgnoreWhitespace) r) => new Regexp(r.Pattern, r.iIgnoreCase, r.xIgnoreWhitespace);
            public static implicit operator Regexp((string Pattern, bool iIgnoreCase, bool xIgnoreWhitespace, bool mMatchMultiline) r) => new Regexp(r.Pattern, r.iIgnoreCase, r.xIgnoreWhitespace, r.mMatchMultiline);
        }
    }
}
