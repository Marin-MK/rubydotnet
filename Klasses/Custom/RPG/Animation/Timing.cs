using System;
using System.Collections.Generic;
using System.Text;

namespace RubyDotNET
{
    public class Timing : RubyObject
    {
        public static Class CreateClass()
        {
            Class c = new Class("Timing", null, Internal.GetKlass("Animation"));
            return c;
        }

        public Timing(IntPtr Pointer)
            : base(Pointer)
        {

        }

        public Timing()
        {

        }

        public RubyInt Frame
        { 
            get
            {
                return new RubyInt(GetIVar("@frame"));
            } 
        }

        public AudioFile SE
        {
            get
            {
                return new AudioFile(GetIVar("@se"));
            }
        }

        public RubyInt FlashScope
        {
            get
            {
                return new RubyInt(GetIVar("@flash_scope"));
            }
        }

        public Color FlashColor
        {
            get
            {
                return new Color(GetIVar("@flash_color"));
            }
        }

        public RubyInt FlashDuration
        {
            get
            {
                return new RubyInt(GetIVar("@flash_duration"));
            }
        }

        public RubyInt Condition
        {
            get
            {
                return new RubyInt(GetIVar("@condition"));
            }
        }
    }
}
