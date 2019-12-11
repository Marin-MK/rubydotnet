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
            c.DefineClassMethod("_load", _load);
            return c;
        }

        public static IntPtr _load(int Argc, IntPtr[] Argv, IntPtr Self)
        {
            return Internal.rb_str_new_cstr("Tone._load");
        }
    }
}
