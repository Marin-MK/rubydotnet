using System;
using System.Collections.Generic;
using System.Text;

namespace rubydotnet
{
    public static partial class Ruby
    {
        public class Integer : Object
        {
            public new static string KlassName = "Integer";
            public new static Class Class { get { return (Class) GetKlass(KlassName); } }

            public Integer(IntPtr Pointer) : base(Pointer, true) { }

            public Integer(int Value) : base(INT2NUM(Value), false) { }

            public Integer(long Value) : base(LONG2NUM(Value), false) { }

            public int ToInt32()
            {
                return NUM2INT(this.Pointer);
            }
            public long ToInt64()
            {
                return NUM2LONG(this.Pointer);
            }
            public override string ToString()
            {
                return ToInt64().ToString();
            }

            public static Integer operator +(Integer One, Integer Two)
            {
                return new Integer(One.ToInt64() + Two.ToInt64());
            }
            public static Integer operator +(Integer One, int Two)
            {
                return new Integer(One.ToInt64() + Two);
            }
            public static Integer operator +(Integer One, long Two)
            {
                return new Integer(One.ToInt64() + Two);
            }
            public static Integer operator +(int One, Integer Two)
            {
                return new Integer(One + Two.ToInt64());
            }
            public static Integer operator +(long One, Integer Two)
            {
                return new Integer(One + Two.ToInt64());
            }
            public static Integer operator ++(Integer One)
            {
                return new Integer(One.ToInt64() + 1);
            }
            
            public static Integer operator -(Integer One, Integer Two)
            {
                return new Integer(One.ToInt64() - Two.ToInt64());
            }
            public static Integer operator -(Integer One, int Two)
            {
                return new Integer(One.ToInt64() - Two);
            }
            public static Integer operator -(Integer One, long Two)
            {
                return new Integer(One.ToInt64() - Two);
            }
            public static Integer operator -(int One, Integer Two)
            {
                return new Integer(One - Two.ToInt64());
            }
            public static Integer operator -(long One, Integer Two)
            {
                return new Integer(One - Two.ToInt64());
            }
            public static Integer operator --(Integer One)
            {
                return new Integer(One.ToInt64() - 1);
            }

            public static Integer operator *(Integer One, Integer Two)
            {
                return new Integer(One.ToInt64() * Two.ToInt64());
            }
            public static Integer operator *(Integer One, int Two)
            {
                return new Integer(One.ToInt64() * Two);
            }
            public static Integer operator *(Integer One, long Two)
            {
                return new Integer(One.ToInt64() * Two);
            }
            public static Integer operator *(int One, Integer Two)
            {
                return new Integer(One * Two.ToInt64());
            }
            public static Integer operator *(long One, Integer Two)
            {
                return new Integer(One * Two.ToInt64());
            }

            public static Integer operator /(Integer One, Integer Two)
            {
                return new Integer(One.ToInt64() / Two.ToInt64());
            }
            public static Integer operator /(Integer One, int Two)
            {
                return new Integer(One.ToInt64() / Two);
            }
            public static Integer operator /(Integer One, long Two)
            {
                return new Integer(One.ToInt64() / Two);
            }
            public static Integer operator /(int One, Integer Two)
            {
                return new Integer(One / Two.ToInt64());
            }
            public static Integer operator /(long One, Integer Two)
            {
                return new Integer(One / Two.ToInt64());
            }

            public static Integer operator %(Integer One, Integer Two)
            {
                return new Integer(One.ToInt64() % Two.ToInt64());
            }
            public static Integer operator %(Integer One, int Two)
            {
                return new Integer(One.ToInt64() % Two);
            }
            public static Integer operator %(Integer One, long Two)
            {
                return new Integer(One.ToInt64() % Two);
            }
            public static Integer operator %(int One, Integer Two)
            {
                return new Integer(One % Two.ToInt64());
            }
            public static Integer operator %(long One, Integer Two)
            {
                return new Integer(One % Two.ToInt64());
            }

            public static Integer operator |(Integer One, Integer Two)
            {
                return new Integer(One.ToInt64() | Two.ToInt64());
            }
            public static Integer operator |(Integer One, int Two)
            {
#pragma warning disable CS0675 // Bitwise-or operator used on a sign-extended operand
                return new Integer(One.ToInt64() | Two);
#pragma warning restore CS0675 // Bitwise-or operator used on a sign-extended operand
            }
            public static Integer operator |(Integer One, long Two)
            {
                return new Integer(One.ToInt64() | Two);
            }
            public static Integer operator |(int One, Integer Two)
            {
#pragma warning disable CS0675 // Bitwise-or operator used on a sign-extended operand
                return new Integer(One | Two.ToInt64());
#pragma warning restore CS0675 // Bitwise-or operator used on a sign-extended operand
            }
            public static Integer operator |(long One, Integer Two)
            {
                return new Integer(One | Two.ToInt64());
            }

            public static Integer operator &(Integer One, Integer Two)
            {
                return new Integer(One.ToInt64() & Two.ToInt64());
            }
            public static Integer operator &(Integer One, int Two)
            {
                return new Integer(One.ToInt64() & Two);
            }
            public static Integer operator &(Integer One, long Two)
            {
                return new Integer(One.ToInt64() & Two);
            }
            public static Integer operator &(int One, Integer Two)
            {
                return new Integer(One & Two.ToInt64());
            }
            public static Integer operator &(long One, Integer Two)
            {
                return new Integer(One & Two.ToInt64());
            }

            public static Integer operator ^(Integer One, Integer Two)
            {
                return new Integer(One.ToInt64() ^ Two.ToInt64());
            }
            public static Integer operator ^(Integer One, int Two)
            {
                return new Integer(One.ToInt64() ^ Two);
            }
            public static Integer operator ^(Integer One, long Two)
            {
                return new Integer(One.ToInt64() ^ Two);
            }
            public static Integer operator ^(int One, Integer Two)
            {
                return new Integer(One ^ Two.ToInt64());
            }
            public static Integer operator ^(long One, Integer Two)
            {
                return new Integer(One ^ Two.ToInt64());
            }

            public static implicit operator int(Integer i) => i.ToInt32();
            public static implicit operator long(Integer i) => i.ToInt64();
            public static implicit operator Integer(int i) => new Integer(i);
            public static implicit operator Integer(long l) => new Integer(l);
        }
    }
}
