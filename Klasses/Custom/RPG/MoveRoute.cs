using System;
using System.Collections.Generic;
using System.Text;

namespace RubyDotNET
{
    public class MoveRoute : RubyObject
    {
        public static Class CreateClass()
        {
            Class c = new Class("MoveRoute", null, Internal.GetKlass("RPG"));
            return c;
        }

        public MoveRoute(IntPtr Pointer)
            : base(Pointer)
        {

        }

        public MoveRoute()
        {

        }

        public bool Repeat
        {
            get
            {
                return GetIVar("@repeat") == Internal.QTrue;
            }
        }

        public bool Skippable
        {
            get
            {
                return GetIVar("@skippable") == Internal.QTrue;
            }
        }

        public RubyArray List
        {
            get
            {
                return new RubyArray(GetIVar("@list"));
            }
        }
    }
}
