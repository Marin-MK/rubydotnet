using System;
using System.Collections.Generic;
using System.Text;

namespace RubyDotNET
{
    public class MapInfo : RubyObject
    {
        public static Class CreateClass()
        {
            Class c = new Class("MapInfo", null, Internal.GetKlass("RPG"));
            return c;
        }

        public MapInfo(IntPtr Pointer)
            : base(Pointer)
        {

        }

        public MapInfo()
        {

        }

        public RubyString Name
        {
            get
            {
                return new RubyString(GetIVar("@name"));
            }
        }

        public RubyInt ParentID
        {
            get
            {
                return new RubyInt(GetIVar("@parent_id"));
            }
        }

        public RubyInt Order
        {
            get
            {
                return new RubyInt(GetIVar("@order"));
            }
        }

        public bool Expanded
        {
            get
            {
                return GetIVar("@expanded") == Internal.QTrue;
            }
        }

        public RubyInt ScrollX
        {
            get
            {
                return new RubyInt(GetIVar("@scroll_x"));
            }
        }

        public RubyInt ScrollY
        {
            get
            {
                return new RubyInt(GetIVar("@scroll_y"));
            }
        }
    }
}
