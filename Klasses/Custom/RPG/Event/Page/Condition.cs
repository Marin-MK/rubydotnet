using System;
using System.Collections.Generic;
using System.Text;

namespace RubyDotNET
{
    public class Condition : RubyObject
    {
        public static Class CreateClass()
        {
            Class c = new Class("Condition", null, Internal.GetKlass("Page"));
            return c;
        }

        public Condition(IntPtr Pointer)
            : base(Pointer)
        {

        }

        public Condition()
        {

        }

        public bool Switch1Valid
        { 
            get
            {
                return GetIVar("@switch1_valid") == Internal.QTrue;
            }
        }

        public bool Switch2Valid
        { 
            get
            {
                return GetIVar("@switch2_valid") == Internal.QTrue;
            }
        }

        public bool VariableValid
        {
            get
            {
                return GetIVar("@variable_valid") == Internal.QTrue;
            }
        }

        public bool SelfSwitchValid
        {
            get
            {
                return GetIVar("@self_switch_valid") == Internal.QTrue;
            }
        }

        public RubyInt Switch1ID
        {
            get
            {
                return new RubyInt(GetIVar("@switch1_id"));
            }
        }

        public RubyInt Switch2ID
        {
            get
            {
                return new RubyInt(GetIVar("@switch2_id"));
            }
        }

        public RubyInt VariableID
        {
            get
            {
                return new RubyInt(GetIVar("@variable_id"));
            }
        }

        public RubyInt VariableValue
        {
            get
            {
                return new RubyInt(GetIVar("@variable_value"));
            }
        }

        public RubyString SelfSwitchChar
        {
            get
            {
                return new RubyString(GetIVar("@self_switch_ch"));
            }
        }
    }
}
