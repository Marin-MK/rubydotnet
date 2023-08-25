using System;

namespace rubydotnet;

public static partial class Ruby
{
    public static class Class
    {
        public static IntPtr Define(string Name, IntPtr? Inherited = null)
        {
            IntPtr Class = rb_define_class(Name, Inherited ?? Object.Class);
            DefineMethod(Class, "new", delegate (IntPtr Self, IntPtr Args)
            {
                IntPtr o = Allocate(Class);
                Funcall(o, "initialize", Args);
                return o;
            });
            return Class;
        }

        public static IntPtr Define(string Name, IntPtr Parent, IntPtr? Inherited = null)
        {
            IntPtr Class = rb_define_class_under(Parent, Name, Inherited ?? Object.Class);
            DefineMethod(Class, "new", delegate (IntPtr Self, IntPtr Args)
            {
                IntPtr o = Allocate(Class);
                Funcall(o, "initialize", Args);
                return o;
            });
            return Class;
        }

        public static IntPtr Allocate(IntPtr Class)
        {
            return Funcall(Class, "allocate");
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
