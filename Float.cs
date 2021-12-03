using System;

namespace rubydotnet;

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
            return (int)Math.Round(FromPtr(Value));
        }

        internal static RB_PtrDbl rb_float_new;
        internal static RB_DblPtr rb_num2dbl;
    }
}
