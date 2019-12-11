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
            c.DefineMethod("width", width);
            c.DefineMethod("height", height);
            c.DefineMethod("tileset_id", tileset_id);
            c.DefineMethod("autoplay_bgm", autoplay_bgm);
            c.DefineMethod("autoplay_bgs", autoplay_bgs);
            c.DefineMethod("encounter_step", encounter_step);
            c.DefineMethod("bgm", bgm);
            c.DefineMethod("bgs", bgs);
            c.DefineMethod("bgs", encounter_list);
            return c;
        }

        public Map()
        {

        }

        public Map(IntPtr ptr)
            : base(ptr)
        {
            
        }

        #region Ruby
        public static IntPtr width(int Argc, IntPtr[] Argv, IntPtr Self)
        {
            return Internal.rb_ivar_get(Self, Internal.rb_intern("@width"));
        }

        public static IntPtr height(int Argc, IntPtr[] Argv, IntPtr Self)
        {
            return Internal.rb_ivar_get(Self, Internal.rb_intern("@height"));
        }

        public static IntPtr tileset_id(int Argc, IntPtr[] Argv, IntPtr Self)
        {
            return Internal.rb_ivar_get(Self, Internal.rb_intern("@tileset_id"));
        }

        public static IntPtr autoplay_bgm(int Argc, IntPtr[] Argv, IntPtr Self)
        {
            return Internal.rb_ivar_get(Self, Internal.rb_intern("@autoplay_bgm"));
        }

        public static IntPtr autoplay_bgs(int Argc, IntPtr[] Argv, IntPtr Self)
        {
            return Internal.rb_ivar_get(Self, Internal.rb_intern("@autoplay_bgs"));
        }

        public static IntPtr encounter_step(int Argc, IntPtr[] Argv, IntPtr Self)
        {
            return Internal.rb_ivar_get(Self, Internal.rb_intern("@encounter_step"));
        }

        public static IntPtr bgm(int Argc, IntPtr[] Argv, IntPtr Self)
        {
            return Internal.rb_ivar_get(Self, Internal.rb_intern("@bgm"));
        }

        public static IntPtr bgs(int Argc, IntPtr[] Argv, IntPtr Self)
        {
            return Internal.rb_ivar_get(Self, Internal.rb_intern("@bgs"));
        }

        public static IntPtr encounter_list(int Argc, IntPtr[] Argv, IntPtr Self)
        {
            return Internal.rb_ivar_get(Self, Internal.rb_intern("@encounter_list"));
        }
        #endregion

        #region C#
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
        #endregion

        /*
#<RPG::Map:0xda0a5b0
  @bgm = #<RPG::AudioFile:0xda0a538
    @volume = 80,
    @name = "021-Field04",
    @pitch = 100
  >,
  @tileset_id = 1,
  @bgs = #<RPG::AudioFile:0xd9f8040
    @volume = 80,
    @name = "",
    @pitch = 100
  >,
  @autoplay_bgm = true,
  @data = #<Table:0xd9f7f98>,
  @autoplay_bgs = false,
  @height = 21,
  @encounter_step = 30,
  @width = 32,
  @encounter_list = []
  @events =
  {
    5 => #<RPG::Event:0xd9d82d8
      @pages = [
        #<RPG::Event::Page:0xd9d8260
          @move_type = 0,
          @list = [
            #<RPG::EventCommand:0xd9d81e8
              @parameters = [".noshadow"],
              @indent = 0,
              @code = 108
            >,
            #<RPG::EventCommand:0xd9d8170
              @parameters = [],
              @indent = 0,
              @code = 0
            >
          ],
          @condition = #<RPG::Event::Page::Condition:0xd9d80e0
            @switch2_valid = false,
            @self_switch_ch = "A",
            @switch1_id = 1,
            @switch1_valid = false,
            @variable_value = 0,
            @self_switch_valid = false,
            @variable_id = 1,
            @variable_valid = false,
            @switch2_id = 1
        >,
        @direction_fix = false,
        @move_route = #<RPG::MoveRoute:0xd9d8080
          @list = [
            #<RPG::MoveCommand:0xd9d8020
              @parameters = [],
              @code = 0
            >
          ],
          @skippable = false,
          @repeat = true
        >,
        @trigger = 0,
        @step_anime = false,
        @move_frequency = 3,
        @graphic = #<RPG::Event::Page::Graphic:0xd9d7f90
          @opacity = 255,
          @character_name = "trchar012",
          @pattern = 0,
          @tile_id = 0,
          @direction = 6,
          @blend_type = 0,
          @character_hue = 0
        >,
        @always_on_top = false,
        @walk_anime = true,
        @move_speed = 3,
        @through = false
      >
    ],
    @name = "EV005",
    @y = 9,
    @x = 9,
    @id = 5
  >}
>
        */
    }
}
