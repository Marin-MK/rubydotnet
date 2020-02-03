using System;
using System.Collections.Generic;
using System.Text;

namespace RubyDotNET
{
    public class Frame : RubyObject
    {
        public static Class CreateClass()
        {
            Class c = new Class("Frame", null, Internal.GetKlass("Animation"));
            return c;
        }

        public Frame(IntPtr Pointer)
            : base(Pointer)
        {

        }

        public Frame()
        {

        }

        public RubyInt CellMax
        {
            get
            {
                return new RubyInt(GetIVar("@cell_max"));
            }
        }

        public Table CellData
        {
            get
            {
                return new Table(GetIVar("@cell_data"));
            }
        }
    }
}
