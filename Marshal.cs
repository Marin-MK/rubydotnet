using System;
using System.Collections.Generic;
using System.Text;

namespace rubydotnet
{
    public static partial class Ruby
    {
        public class Marshal : Object
        {
            public new static string KlassName = "Marshal";
            public static Module Module { get => (Module) GetKlass(KlassName); }

            protected Marshal(IntPtr Pointer) : base(Pointer, true) { }

            public static Object Load(String Data)
            {
                return new Object(SafeRuby(delegate (IntPtr Arg) { return rb_marshal_load(Data.Pointer); }), false);
            }
            public static Object Load(File File)
            {
                return new Object(SafeRuby(delegate (IntPtr Arg) { return rb_marshal_load(File.Pointer); }), false);
            }

            public static String Dump(Object Data)
            {
                return new String(SafeRuby(delegate (IntPtr Arg) { return rb_marshal_dump(Data.Pointer, QNil); }));
            }
            public static Object Dump(Object Data, File File)
            {
                return new Object(SafeRuby(delegate (IntPtr Arg) { return rb_marshal_dump(Data.Pointer, File.Pointer); }), false);
            }
        }
    }
}
