using System;
using System.Collections.Generic;
using System.Text;

namespace RubyDotNET
{
    public class Map : RubyObject
    {
        public static Class CreateClass()
        {
            Class c = new Class("Map", null, Internal.GetKlass("RPG"));
            /*c.DefineMethod("width", width);
            c.DefineMethod("height", height);
            c.DefineMethod("tileset_id", tileset_id);
            c.DefineMethod("autoplay_bgm", autoplay_bgm);
            c.DefineMethod("autoplay_bgs", autoplay_bgs);
            c.DefineMethod("encounter_step", encounter_step);
            c.DefineMethod("bgm", bgm);
            c.DefineMethod("bgs", bgs);
            c.DefineMethod("encounter_list", encounter_list);*/
            return c;
        }

        public Map(IntPtr Pointer)
            : base(Pointer)
        {

        }

        public Map()
        {

        }

        public RubyInt Width
        {
            get
            {
                return new RubyInt(GetIVar("@width"));
            }
        }

        public RubyInt Height
        {
            get
            {
                return new RubyInt(GetIVar("@height"));
            }
        }

        public RubyInt TilesetID
        {
            get
            {
                return new RubyInt(GetIVar("@tileset_id"));
            }
        }

        public bool AutoplayBGM
        {
            get
            {
                return GetIVar("@autoplay_bgm") == Internal.QTrue;
            }
        }

        public bool AutoplayBGS
        {
            get
            {
                return GetIVar("@autoplay_bgs") == Internal.QTrue;
            }
        }

        public RubyInt EncounterStep
        {
            get
            {
                return new RubyInt(GetIVar("@encounter_step"));
            }
        }

        public AudioFile BGM
        {
            get
            {
                return new AudioFile(GetIVar("@bgm"));
            }
        }

        public AudioFile BGS
        {
            get
            {
                IntPtr ptr = GetIVar("@bgs");
                if (ptr == Internal.QNil) return null;
                return new AudioFile(ptr);
            }
        }

        public RubyArray EncounterList
        {
            get
            {
                return new RubyArray(GetIVar("@encounter_list"));
            }
        }

        public RubyArray Data
        {
            get
            {
                return new RubyArray(GetIVar("@data"));
            }
        }

        public RubyHash Events
        {
            get
            {
                return new RubyHash(GetIVar("@events"));
            }
        }
    }
}
