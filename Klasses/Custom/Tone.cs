using System;
using System.Collections.Generic;
using System.Text;

namespace RubyDotNET
{
    public class Tone : RubyObject
    {
        public static Class CreateClass()
        {
            Class c = new Class("Tone");
            c.DefineClassMethod("_load", _load, -2);
            return c;
        }

        public static IntPtr _load(IntPtr _self, IntPtr _args)//int argc, IntPtr[] argv, IntPtr self)
        {
            return Internal.rb_str_new_cstr("Tone._load");
        }
    }
}
