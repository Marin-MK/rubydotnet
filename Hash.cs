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

            internal static RB_Ptr rb_hash_new;
            internal static RB_PtrPtr rb_hash_keys;
            internal static RB_PtrPtr rb_hash_values;
            internal static RB_PtrPtrPtr rb_hash_aref;
            internal static RB_PtrPtrPtrPtr rb_hash_aset;
            internal static RB_PtrPtr rb_hash_size;
        }
    }
}
