using System;

namespace rubydotnet
{
    public static partial class Ruby
    {
        public static class Module
        {
            public static IntPtr Define(string Name)
            {
                return rb_define_module(Name);
            }
            
            public static IntPtr Define(string Name, IntPtr Parent)
            {
                return rb_define_module_under(Parent, Name);
            }

            public static void DefineMethod(IntPtr Class, string Name, RubyMethod Method)
            {
                Object.MethodCache.Add(Method);
                rb_define_method(Class, Name, Method, -2);
            }

            public static void DefineClassMethod(IntPtr Class, string Name, RubyMethod Method)
            {
                Object.MethodCache.Add(Method);
                rb_define_singleton_method(Class, Name, Method, -2);
            }
        }
    }
}
