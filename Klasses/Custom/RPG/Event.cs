using System;
using System.Collections.Generic;
using System.Text;

namespace RubyDotNET
{
    public class Event : RubyObject
    {
        public static Class CreateClass()
        {
            Class c = new Class("Event", null, Internal.GetKlass("RPG"));
            return c;
        }

        public Event(IntPtr Pointer)
            : base(Pointer)
        {

        }

        public Event()
        {

        }

        public RubyInt ID
        {
            get
            {
                return new RubyInt(GetIVar("@id"));
            }
        }

        public RubyString Name
        {
            get
            {
                return new RubyString(GetIVar("@name"));
            }
        }

        public RubyInt X
        {
            get
            {
                return new RubyInt(GetIVar("@x"));
            }
        }

        public RubyInt Y
        {
            get
            {
                return new RubyInt(GetIVar("@y"));
            }
        }

        public RubyArray Pages
        {
            get
            {
                return new RubyArray(GetIVar("@pages"));
            }
        }
    }
}
