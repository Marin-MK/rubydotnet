using System;

namespace rubydotnet
{
    public static partial class Ruby
    {
        public static class Symbol
        {
            public static IntPtr ToPtr(string Value)
            {
                return rb_str_intern(String.ToPtr(Value));
            }

            public static string FromPtr(IntPtr Ptr)
            {
                return String.FromPtr(Funcall(Ptr, "to_s"));
            }
        }
    }
}
