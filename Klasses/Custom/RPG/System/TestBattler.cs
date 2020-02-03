using System;
using System.Collections.Generic;
using System.Text;

namespace RubyDotNET
{
    public class TestBattler : RubyObject
    {
        public static Class CreateClass()
        {
            Class c = new Class("TestBattler", null, Internal.GetKlass("System"));
            return c;
        }

        public TestBattler(IntPtr Pointer)
            : base(Pointer)
        {

        }

        public TestBattler()
        {

        }

        public RubyInt ActorID
        {
            get
            {
                return new RubyInt(GetIVar("@actor_id"));
            }
        }

        public RubyInt Level
        {
            get
            {
                return new RubyInt(GetIVar("@level"));
            }
        }

        public RubyInt WeaponID
        {
            get
            {
                return new RubyInt(GetIVar("@weapon_id"));
            }
        }

        public RubyInt Armor1ID
        {
            get
            {
                return new RubyInt(GetIVar("@armor1_id"));
            }
        }

        public RubyInt Armor2ID
        {
            get
            {
                return new RubyInt(GetIVar("@armor2_id"));
            }
        }

        public RubyInt Armor3ID
        {
            get
            {
                return new RubyInt(GetIVar("@armor3_id"));
            }
        }

        public RubyInt Armor4ID
        {
            get
            {
                return new RubyInt(GetIVar("@armor4_id"));
            }
        }
    }
}
