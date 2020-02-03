using System;
using System.Collections.Generic;
using System.Text;

namespace RubyDotNET
{
    public class Color : RubyObject
    {
        public static IntPtr ClassPointer;

        public static Class CreateClass()
        {
            Class c = new Class("Color");
            ClassPointer = c.Pointer;
            c.DefineClassMethod("_load", _load, -2);
            return c;
        }

        public Color(IntPtr Pointer)
            : base(Pointer)
        {

        }

        public Color()
        {

        }

        public static IntPtr _load(IntPtr _self, IntPtr _args)//int argc, IntPtr[] argv, IntPtr self)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            //IntPtr[] Values = new IntPtr[1];

            //Internal.rb_scan_args(argc, argv, "1", Values);

            RubyArray ary = new RubyArray(Internal.rb_funcallv(Args[0].Pointer, Internal.rb_intern("unpack"), 1, Internal.rb_str_new_cstr("D*")));

            IntPtr obj = Internal.rb_funcallv(Color.ClassPointer, Internal.rb_intern("new"), 0);

            Internal.rb_ivar_set(obj, Internal.rb_intern("@red"), ary[0].Pointer);
            Internal.rb_ivar_set(obj, Internal.rb_intern("@green"), ary[1].Pointer);
            Internal.rb_ivar_set(obj, Internal.rb_intern("@blue"), ary[2].Pointer);
            Internal.rb_ivar_set(obj, Internal.rb_intern("@alpha"), ary[3].Pointer);
            
            return obj;
        }
    }
}
