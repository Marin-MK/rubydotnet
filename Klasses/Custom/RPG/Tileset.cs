using System;
using System.Collections.Generic;
using System.Text;

namespace RubyDotNET
{
    public class Tileset : RubyObject
    {
        public static Class CreateClass()
        {
            Class c = new Class("Tileset", null, Internal.GetKlass("RPG"));
            return c;
        }

        public Tileset(IntPtr Pointer)
            : base(Pointer)
        {

        }

        public Tileset()
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

        public RubyString TilesetName
        {
            get
            {
                return new RubyString(GetIVar("@tileset_name"));
            }
        }

        public RubyArray AutotileNames
        {
            get
            {
                return new RubyArray(GetIVar("@autotile_names"));
            }
        }

        public RubyString PanoramaName
        {
            get
            {
                return new RubyString(GetIVar("@panorama_name"));
            }
        }

        public RubyInt PanoramaHue
        {
            get
            {
                return new RubyInt(GetIVar("@panorama_hue"));
            }
        }

        public RubyString FogName
        {
            get
            {
                return new RubyString(GetIVar("@fog_name"));
            }
        }

        public RubyInt FogHue
        {
            get
            {
                return new RubyInt(GetIVar("@fog_hue"));
            }
        }

        public RubyInt FogOpacity
        {
            get
            {
                return new RubyInt(GetIVar("@fog_opacity"));
            }
        }

        public RubyInt FogBlendType
        {
            get
            {
                return new RubyInt(GetIVar("@fog_blend_type"));
            }
        }

        public RubyInt FogZoom
        {
            get
            {
                return new RubyInt(GetIVar("@fog_zoom"));
            }
        }

        public RubyInt FogSX
        {
            get
            {
                return new RubyInt(GetIVar("@fog_sx"));
            }
        }

        public RubyInt FogSY
        {
            get
            {
                return new RubyInt(GetIVar("@fog_sy"));
            }
        }

        public RubyString BattlebackName
        { 
            get
            {
                return new RubyString(GetIVar("@battleback_name"));
            }
        }

        public Table Passages
        {
            get
            {
                return new Table(GetIVar("@passages"));
            }
        }

        public Table Priorities
        {
            get
            {
                return new Table(GetIVar("@priorities"));
            }
        }

        public Table TerrainTags
        {
            get
            {
                return new Table(GetIVar("@terrain_tags"));
            }
        }
    }
}
