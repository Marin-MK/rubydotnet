using System;
using System.Runtime.InteropServices;

namespace rubydotnet;

public static partial class Ruby
{
    public static class Marshal
    {
        public static IntPtr Load(IntPtr Data)
        {
            IntPtr ret = IntPtr.Zero;
            SafeRuby(_ =>
            {
                ret = rb_marshal_load(Data);
                return IntPtr.Zero;
            });
            return ret;
        }

        public static IntPtr Dump(IntPtr Data)
        {
            return rb_marshal_dump(Data, Ruby.Nil);
        }

        public static IntPtr Dump(IntPtr Data, IntPtr IO)
        {
            return rb_marshal_dump(Data, IO);
        }

        internal static RB_PtrPtr rb_marshal_load;
        internal static RB_PtrPtrPtr rb_marshal_dump;
    }
}
