using System;
using System.Runtime.InteropServices;

namespace rubydotnet
{
    public static partial class Ruby
    {
        public static class Integer
        {
            public static IntPtr ToPtr(long Value)
            {
                if (RB_FIXABLE(Value)) return RB_INT2FIX(Value);
                else return rb_int2big(Value);
            }

            public static long FromPtr(IntPtr Value)
            {
                if (RB_FIXNUM_P(Value)) return (int) Value >> 1;
                return rb_num2ll(Value);
            }

            static bool RB_FIXABLE(long f)
            {
                return RB_POSFIXABLE(f) && RB_NEGFIXABLE(f);
            }
            static bool RB_POSFIXABLE(long f)
            {
                return f < RUBY_FIXNUM_MAX + 1;
            }
            static bool RB_NEGFIXABLE(long f)
            {
                return f >= RUBY_FIXNUM_MIN;
            }
            static bool RB_FIXNUM_P(IntPtr v)
            {
                return (((int) (long) (v)) & RUBY_FIXNUM_FLAG) == 1;
            }
            static IntPtr RB_INT2FIX(long v)
            {
#pragma warning disable CS0675 // Bitwise-or operator used on a sign-extended operand
                return (IntPtr) ((v << 1) | RUBY_FIXNUM_FLAG);
#pragma warning restore CS0675 // Bitwise-or operator used on a sign-extended operand
            }

            static int LONG_MAX = 2147483647;
            static int LONG_MIN = -LONG_MAX - 1;
            static int RUBY_FIXNUM_FLAG = 0x01;
            static int RUBY_FIXNUM_MAX = LONG_MAX >> 1;
            static int RUBY_FIXNUM_MIN = LONG_MIN >> 1;

            [DllImport(RubyPath)]
            static extern IntPtr rb_int2big(long Value);

            [DllImport(RubyPath)]
            static extern long rb_num2ll(IntPtr Value);
        }
    }
}
