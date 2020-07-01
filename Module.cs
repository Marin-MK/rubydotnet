using System;
using System.Collections.Generic;
using System.Text;

namespace rubydotnet
{
    public static partial class Ruby
    {
        public class Module : Object
        {
            public static Dictionary<Module, Type> CustomModules = new Dictionary<Module, Type>();

            public string Name;

            public Module(IntPtr Pointer, string Name) : base(Pointer, true)
            {
                this.Name = Name;
            }

            public static Module DefineModule<T>(string Name, Object UnderKlass = null) where T : Object
            {
                if (UnderKlass != null && !(UnderKlass is Class) && !(UnderKlass is Module)) throw new Exception("Invalid parent klass");
                IntPtr modptr = IntPtr.Zero;
                if (UnderKlass == null) modptr = rb_define_module(Name);
                else modptr = rb_define_module_under(UnderKlass.Pointer, Name);
                Module mod = new Module(modptr, Name);
                CustomModules.Add(mod, typeof(T));
                return mod;
            }
            public static Module DefineModule<T>(string Name, string UnderKlass = null) where T : Object
            {
                return DefineModule<T>(Name, GetKlass(UnderKlass));
            }
            public static Module DefineModule<T>(string Name) where T : Object
            {
                return DefineModule<T>(Name, (Module) null);
            }

            List<object> MethodCache = new List<object>();

            public override string ToString()
            {
                return Name;
            }

            public void DefineMethod(string Name, Method Method)
            {
                InternalMethod imethod = delegate (IntPtr Self, IntPtr Args)
                {
                    return Method(new Object(Self, false), new Array(Args)).Pointer;
                };
                MethodCache.Add(imethod);
                MethodCache.Add(Method);
                rb_define_method(this.Pointer, Name, imethod, -2);
            }

            public void DefineClassMethod(string Name, Method Method)
            {
                InternalMethod imethod = delegate (IntPtr Self, IntPtr Args)
                {
                    return Method(new Module(Self, KlassName), new Array(Args)).Pointer;
                };
                MethodCache.Add(imethod);
                MethodCache.Add(Method);
                rb_define_singleton_method(this.Pointer, Name, imethod, -2);
            }
        }
    }
}
