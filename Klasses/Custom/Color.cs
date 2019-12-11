using System;
using System.Collections.Generic;
using System.Text;

namespace RubyDotNET
{
    public class Color : RubyObject
    {
        public static Class CreateClass()
        {
            Class c = new Class("Color");
            return c;
        }
    }
}
