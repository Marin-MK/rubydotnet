using System;
using System.Collections.Generic;
using System.Text;

namespace RubyDotNET
{
    public class CommonEvent : RubyObject
    {
        public static Class CreateClass()
        {
            Class c = new Class("CommonEvent", null, Internal.GetKlass("RPG"));
            return c;
        }

        public CommonEvent(IntPtr Pointer)
            : base(Pointer)
        {

        }

        public CommonEvent()
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

        public RubyInt Trigger
        {
            get
            {
                return new RubyInt(GetIVar("@trigger"));
            }
        }

        public RubyInt SwitchID
        {
            get
            {
                return new RubyInt(GetIVar("@switch_id"));
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
