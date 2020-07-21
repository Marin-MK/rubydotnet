using System;
using System.Runtime.InteropServices;

namespace rubydotnet
{
    public static partial class Ruby
    {
        public static class Float
        {
            public static IntPtr ToPtr(double Value)
            {
                return rb_float_new(Value);
            }

            public static double FromPtr(IntPtr Value)
            {
                return rb_num2dbl(Value);
            }

            public static int RoundFromPtr(IntPtr Value)
            {
                return (int) Math.Round(FromPtr(Value));
            }

            [DllImport(RubyPath)]
            static extern IntPtr rb_float_new(double Value);

            [DllImport(RubyPath)]
            static extern double rb_num2dbl(IntPtr Value);
        }
    }
}
