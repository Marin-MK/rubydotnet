using System;
using System.Collections.Generic;
using System.Text;

namespace RubyDotNET
{
    public class Page : RubyObject
    {
        public static Class CreateClass()
        {
            Class c = new Class("Page", null, Internal.GetKlass("Event"));
            return c;
        }

        public Page(IntPtr Pointer)
            : base(Pointer)
        {

        }

        public Page()
        {

        }

        public Condition Condition
        {
            get
            {
                return new Condition(GetIVar("@condition"));
            }
        }

        public Graphic Graphic
        {
            get
            {
                return new Graphic(GetIVar("@graphic"));
            }
        }

        public RubyInt MoveType
        {
            get
            {
                return new RubyInt(GetIVar("@move_type"));
            }
        }

        public RubyInt MoveSpeed
        {
            get
            {
                return new RubyInt(GetIVar("@move_speed"));
            }
        }

        public RubyInt MoveFrequency
        {
            get
            {
                return new RubyInt(GetIVar("@move_frequency"));
            }
        }

        public MoveRoute MoveRoute
        {
            get
            {
                return new MoveRoute(GetIVar("@move_route"));
            }
        }

        public bool WalkAnime
        {
            get
            {
                return GetIVar("@walk_anime") == Internal.QTrue;
            }
        }

        public bool StepAnime
        {
            get
            {
                return GetIVar("@step_anime") == Internal.QTrue;
            }
        }

        public bool DirectionFix
        {
            get
            {
                return GetIVar("@direction_fix") == Internal.QTrue;
            }
        }

        public bool Through
        {
            get
            {
                return GetIVar("@through") == Internal.QTrue;
            }
        }

        public bool AlwaysOnTop
        {
            get
            {
                return GetIVar("@always_on_top") == Internal.QTrue;
            }
        }

        public RubyInt Trigger
        {
            get
            {
                return new RubyInt(GetIVar("@trigger"));
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
