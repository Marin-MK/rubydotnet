using System;
using System.Collections.Generic;

namespace rubydotnet;

public static partial class Ruby
{
    public static class Object
    {
        public static List<RubyMethod> MethodCache = new List<RubyMethod>();

        private static IntPtr _class = IntPtr.Zero;
        public static IntPtr Class
        {
            get
            {
                if (_class == IntPtr.Zero) _class = rb_path2class("Object");
                return _class;
            }
        }
    }
}
