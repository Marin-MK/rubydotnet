using System;
using System.Collections.Generic;
using System.Text;

namespace RubyDotNET
{
    public class Words : RubyObject
    {
        public static Class CreateClass()
        {
            Class c = new Class("Words", null, Internal.GetKlass("System"));
            return c;
        }

        public Words(IntPtr Pointer)
            : base(Pointer)
        {

        }

        public Words()
        {

        }

        public RubyString Gold
        {
            get
            {
                return new RubyString(GetIVar("@gold"));
            }
        }

        public RubyString HP
        {
            get
            {
                return new RubyString(GetIVar("@hp"));
            }
        }

        public RubyString SP
        {
            get
            {
                return new RubyString(GetIVar("@sp"));
            }
        }

        public RubyString Str
        {
            get
            {
                return new RubyString(GetIVar("@str"));
            }
        }

        public RubyString Dex
        {
            get
            {
                return new RubyString(GetIVar("@dex"));
            }
        }

        public RubyString Agi
        {
            get
            {
                return new RubyString(GetIVar("@agi"));
            }
        }

        public RubyString Int
        {
            get
            {
                return new RubyString(GetIVar("@int"));
            }
        }

        public RubyString Atk
        {
            get
            {
                return new RubyString(GetIVar("@atk"));
            }
        }

        public RubyString PDef
        {
            get
            {
                return new RubyString(GetIVar("@pdef"));
            }
        }

        public RubyString MDef
        {
            get
            {
                return new RubyString(GetIVar("@mdef"));
            }
        }

        public RubyString Weapon
        {
            get
            {
                return new RubyString(GetIVar("@weapon"));
            }
        }

        public RubyString Armor1
        {
            get
            {
                return new RubyString(GetIVar("@armor1"));
            }
        }

        public RubyString Armor2
        {
            get
            {
                return new RubyString(GetIVar("@armor2"));
            }
        }

        public RubyString Armor3
        {
            get
            {
                return new RubyString(GetIVar("@armor3"));
            }
        }

        public RubyString Armor4
        {
            get
            {
                return new RubyString(GetIVar("@armor4"));
            }
        }

        public RubyString Attack
        {
            get
            {
                return new RubyString(GetIVar("@attack"));
            }
        }

        public RubyString Skill
        {
            get
            {
                return new RubyString(GetIVar("@skill"));
            }
        }

        public RubyString Guard
        {
            get
            {
                return new RubyString(GetIVar("@guard"));
            }
        }

        public RubyString Item
        {
            get
            {
                return new RubyString(GetIVar("@item"));
            }
        }

        public RubyString Equip
        {
            get
            {
                return new RubyString(GetIVar("@equip"));
            }
        }
    }
}
