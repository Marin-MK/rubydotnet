using System;
using System.Collections.Generic;
using System.Text;

namespace RubyDotNET
{
    public class Graphic : RubyObject
    {
        public static Class CreateClass()
        {
            Class c = new Class("Graphic", null, Internal.GetKlass("Page"));
            return c;
        }

        public Graphic(IntPtr Pointer)
            : base(Pointer)
        {

        }

        public Graphic()
        {

        }

        public RubyInt TileID
        {
            get
            {
                return new RubyInt(GetIVar("@tile_id"));
            }
        }

        public RubyString CharacterName
        { 
            get
            {
                return new RubyString(GetIVar("@character_name"));
            } 
        }

        public RubyInt CharacterHue
        {
            get
            {
                return new RubyInt(GetIVar("@character_hue"));
            }
        }

        public RubyInt Direction
        {
            get
            {
                return new RubyInt(GetIVar("@direction"));
            }
        }

        public RubyInt Pattern
        {
            get
            {
                return new RubyInt(GetIVar("@pattern"));
            }
        }

        public RubyInt Opacity
        {
            get
            {
                return new RubyInt(GetIVar("@opacity"));
            }
        }

        public RubyInt BlendType
        { 
            get
            {
                return new RubyInt(GetIVar("@blend_type"));
            }
        }
    }
}
