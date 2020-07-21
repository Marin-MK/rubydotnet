using System;
using System.Runtime.InteropServices;

namespace rubydotnet
{
    public static partial class Ruby
    {
        public static class Hash
        {
            public static IntPtr Keys(IntPtr Object)
            {
                return rb_hash_keys(Object);
            }

            public static IntPtr Values(IntPtr Object)
            {
                return rb_hash_values(Object);
            }

            public static IntPtr Get(IntPtr Object, IntPtr Key)
            {
                return rb_hash_aref(Object, Key);
            }

            public static void Set(IntPtr Object, IntPtr Key, IntPtr Value)
            {
                rb_hash_aset(Object, Key, Value);
            }

            public static int Length(IntPtr Object)
            {
                return (int) Integer.FromPtr(rb_hash_size(Object));
            }

            public static IntPtr Create()
            {
                return rb_hash_new();
            }

            [DllImport(RubyPath)]
            static extern IntPtr rb_hash_new();

            [DllImport(RubyPath)]
            static extern IntPtr rb_hash_keys(IntPtr Object);

            [DllImport(RubyPath)]
            static extern IntPtr rb_hash_values(IntPtr Object);

            [DllImport(RubyPath)]
            static extern IntPtr rb_hash_aref(IntPtr Object, IntPtr Key);

            [DllImport(RubyPath)]
            static extern IntPtr rb_hash_aset(IntPtr Object, IntPtr Key, IntPtr Value);

            [DllImport(RubyPath)]
            static extern IntPtr rb_hash_size(IntPtr Object);
        }
    }
}
