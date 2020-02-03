using System;
using System.Collections.Generic;
using System.Text;

namespace RubyDotNET
{
    public class Animation : RubyObject
    {
        public static Class CreateClass()
        {
            Class c = new Class("Animation", null, Internal.GetKlass("RPG"));
            return c;
        }

        public Animation(IntPtr Pointer)
            : base(Pointer)
        {

        }

        public Animation()
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

        public RubyString AnimationName
        {
            get
            {
                return new RubyString(GetIVar("@animation_name"));
            }
        }

        public RubyInt AnimationHue
        {
            get
            {
                return new RubyInt(GetIVar("@animation_hue"));
            }
        }

        public RubyInt Position
        {
            get
            {
                return new RubyInt(GetIVar("@position"));
            }
        }

        public RubyInt FrameMax
        {
            get
            {
                return new RubyInt(GetIVar("@frame_max"));
            }
        }

        public RubyArray Frames
        {
            get
            {
                return new RubyArray(GetIVar("@frames"));
            }
        }

        public RubyArray Timings
        {
            get
            {
                return new RubyArray(GetIVar("@timings"));
            }
        }
    }
}
