using System;
using System.Collections.Generic;
using System.Text;

namespace rubydotnet
{
    public static partial class Ruby
    {
        public class Float : Object
        {
            public new static string KlassName = "Float";
            public new static Class Class { get { return (Class) GetKlass(KlassName); } }

            public Float(IntPtr Pointer) : base(Pointer, true) { }

            public Float(double Value) : base(DBL2NUM(Value), false) { }

            public double ToDouble()
            {
                return NUM2DBL(this.Pointer);
            }
            public override string ToString()
            {
                return ToDouble().ToString();
            }

            public static Float operator +(Float One, Float Two)
            {
                return new Float(One.ToDouble() + Two.ToDouble());
            }
            public static Float operator +(Float One, Integer Two)
            {
                return new Float(One.ToDouble() + Two.ToInt64());
            }
            public static Float operator +(Integer One, Float Two)
            {
                return new Float(One.ToInt64() + Two.ToDouble());
            }
            public static Float operator ++(Float One)
            {
                return new Float(One.ToDouble() + 1);
            }

            public static Float operator -(Float One, Float Two)
            {
                return new Float(One.ToDouble() - Two.ToDouble());
            }
            public static Float operator -(Float One, Integer Two)
            {
                return new Float(One.ToDouble() - Two.ToInt64());
            }
            public static Float operator -(Integer One, Float Two)
            {
                return new Float(One.ToInt64() - Two.ToDouble());
            }
            public static Float operator --(Float One)
            {
                return new Float(One.ToDouble() - 1);
            }

            public static Float operator *(Float One, Float Two)
            {
                return new Float(One.ToDouble() * Two.ToDouble());
            }
            public static Float operator *(Float One, Integer Two)
            {
                return new Float(One.ToDouble() * Two.ToInt64());
            }
            public static Float operator *(Integer One, Float Two)
            {
                return new Float(One.ToInt64() * Two.ToDouble());
            }

            public static Float operator /(Float One, Float Two)
            {
                return new Float(One.ToDouble() / Two.ToDouble());
            }
            public static Float operator /(Float One, Integer Two)
            {
                return new Float(One.ToDouble() / Two.ToInt64());
            }
            public static Float operator /(Integer One, Float Two)
            {
                return new Float(One.ToInt64() / Two.ToDouble());
            }

            public static Float operator %(Float One, Float Two)
            {
                return new Float(One.ToDouble() % Two.ToDouble());
            }
            public static Float operator %(Float One, Integer Two)
            {
                return new Float(One.ToDouble() % Two.ToInt64());
            }
            public static Float operator %(Integer One, Float Two)
            {
                return new Float(One.ToInt64() % Two.ToDouble());
            }

            public static implicit operator double(Float f) => f.ToDouble();
            public static implicit operator Float(double d) => new Float(d);
        }
    }
}
