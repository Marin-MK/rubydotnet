using System;
using System.Collections.Generic;
using System.Text;

namespace rubydotnet
{
    public static partial class Ruby
    {
        public class Class : Object
        {
            public static Dictionary<Class, Type> CustomClasses = new Dictionary<Class, Type>();

            public string Name;

            public Class(IntPtr Pointer, string Name) : base(Pointer, true)
            {
                this.Name = Name;
            }

            public static Class DefineClass<T>(string Name, Object InheritedClass = null, Object UnderKlass = null) where T : Object
            {
                if (InheritedClass != null && !(InheritedClass is Class))
                {
                    if (InheritedClass is Module) throw new Exception("Cannot inherit from Module.");
                    else throw new Exception($"Invalid superclass type {InheritedClass}.");
                }
                if (UnderKlass != null && !(UnderKlass is Class) && !(UnderKlass is Module)) throw new Exception("Invalid parent klass");
                if (InheritedClass == null) InheritedClass = Object.Class;
                IntPtr classptr = IntPtr.Zero;
                if (UnderKlass == null) classptr = rb_define_class(Name, InheritedClass.Pointer);
                else classptr = rb_define_class_under(UnderKlass.Pointer, Name, InheritedClass.Pointer);
                if (UnderKlass is Class) Name = ((Class) UnderKlass).Name + "::" + Name;
                else if (UnderKlass is Module) Name = ((Module) UnderKlass).Name + "::" + Name;
                Class cls = new Class(classptr, Name);
                CustomClasses.Add(cls, typeof(T));
                cls.DefineClassMethod("new", delegate (Object Self, Array Args)
                {
                    Object obj = ((Class) Self).Allocate();
                    obj.Funcall("initialize", Args);
                    return obj;
                });
                cls.DefineMethod("initialize", delegate (Object Self, Array Args)
                {
                    return Nil;
                });
                return cls;
            }
            public static Class DefineClass<T>(string Name, string InheritedClass = null, string UnderKlass = null) where T : Object
            {
                Object cInheritedClass = string.IsNullOrEmpty(InheritedClass) ? null : GetKlass(InheritedClass);
                Object kUnderKlass = string.IsNullOrEmpty(UnderKlass) ? null : GetKlass(UnderKlass);
                return DefineClass<T>(Name, cInheritedClass, kUnderKlass);
            }
            public static Class DefineClass<T>(string Name) where T : Object
            {
                return DefineClass<T>(Name, (Class) null, null);
            }

            List<object> MethodCache = new List<object>();

            public override string ToString()
            {
                return Name;
            }

            public Object Allocate()
            {
                return Funcall("allocate");
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
                    return Method(new Class(Self, KlassName), new Array(Args)).Pointer;
                };
                MethodCache.Add(imethod);
                MethodCache.Add(Method);
                rb_define_singleton_method(this.Pointer, Name, imethod, -2);
            }
        }
    }
}
