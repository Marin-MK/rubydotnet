using System;
using System.Collections.Generic;
using System.Text;

namespace RubyDotNET
{
    public class MoveCommand : RubyObject
    {
        public static Class CreateClass()
        {
            Class c = new Class("MoveCommand", null, Internal.GetKlass("RPG"));
            return c;
        }

        public MoveCommand(IntPtr Pointer)
            : base(Pointer)
        {

        }

        public MoveCommand()
        {

        }

        public RubyInt Code
        {
            get
            {
                return new RubyInt(GetIVar("@code"));
            }
        }

        public RubyArray Parameters
        {
            get
            {
                return new RubyArray(GetIVar("@parameters"));
            }
        }
    }
}
