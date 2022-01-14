using System;
using System.Runtime.InteropServices;

namespace rubydotnet;

public static partial class Ruby
{
    public static class String
    {
        public unsafe static IntPtr ToPtr(string Value)
        {
            if (Value.Length == 0)
            {
                // Likely faster, but cannot convert UTF-8 C# strings back to UTF-8 Ruby strings.
                return rb_str_new(Value, Value.Length);
            }
            else
            {
                // Likely slower, but will retain C# String UTF-8 encoding in the Ruby string.
                IntPtr ret = IntPtr.Zero;
                fixed (byte* p = System.Text.Encoding.UTF8.GetBytes(Value + '\0'))
                {
                    ret = rb_utf8_str_new_cstr((IntPtr) p);
                }
                if (ret == IntPtr.Zero) throw new Exception("Could not convert string to pointer.");
                return ret;
            }
        }

        public static string FromPtr(IntPtr Value)
        {
            long len = Integer.FromPtr(Funcall(Value, "bytesize"));
            return Marshal.PtrToStringUTF8(rb_string_value_ptr(ref Value), (int)len);
        }

        internal static RB_PtrStrInt rb_str_new;
        internal static RB_PtrPtr rb_utf8_str_new_cstr;
        internal static RB_PtrRefPtr rb_string_value_ptr;
    }
}
