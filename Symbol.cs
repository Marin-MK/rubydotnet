using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace rubydotnet
{
    public static partial class Ruby
    {
        public class Symbol : Object
        {
            public new static string KlassName = "Symbol";
            public new static Class Class { get { return (Class) GetKlass(KlassName); } }

            public Symbol(IntPtr Pointer) : base(Pointer, true) { }

            public Symbol(string Symbol) : base(rb_str_intern(STR2NUM(Symbol)), false) { }

            public override string ToString()
            {
                return ":" + Funcall("to_s").Convert<String>();
            }

            public static bool operator ==(Symbol One, Symbol Two)
            {
                return One.ToString() == Two.ToString();
            }
            public static bool operator !=(Symbol One, Symbol Two)
            {
                return One.ToString() != Two.ToString();
            }

            public override bool Equals(object obj)
            {
                if (obj is Symbol)
                {
                    return this == (Symbol) obj;
                }
                return base.Equals(obj);
            }

            public override int GetHashCode()
            {
                return ToString().GetHashCode();
            }

            public static implicit operator Symbol(string Symbol) => new Symbol(Symbol);
        }
    }
}
