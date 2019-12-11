using System;
using System.Collections.Generic;
using System.Text;

namespace RubyDotNET
{
    public class EventCommand : RubyObject
    {
        public static Class CreateClass()
        {
            Class c = new Class("EventCommand", null, Internal.GetKlass("RPG"));
            return c;
        }

        public EventCommand(IntPtr Pointer)
            : base(Pointer)
        {

        }

        public EventCommand()
        {

        }

        public RubyInt Code
        {
            get
            {
                return new RubyInt(GetIVar("@code"));
            }
        }

        public RubyInt Indent
        {
            get
            {
                return new RubyInt(GetIVar("@indent"));
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
