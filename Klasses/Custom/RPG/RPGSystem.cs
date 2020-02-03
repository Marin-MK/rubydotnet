using System;
using System.Collections.Generic;
using System.Text;

namespace RubyDotNET
{
    public class RPGSystem : RubyObject
    {
        public static Class CreateClass()
        {
            Class c = new Class("System", null, Internal.GetKlass("RPG"));
            return c;
        }

        public RPGSystem(IntPtr Pointer)
            : base(Pointer)
        {

        }

        public RPGSystem()
        {

        }

        public RubyInt MagicNumber
        {
            get
            {
                IntPtr ptr = GetIVar("@magic_number");
                if (ptr == Internal.QNil) return null;
                return new RubyInt(ptr);
            }
        }

        public RubyArray PartyMembers
        {
            get
            {
                return new RubyArray(GetIVar("@party_members"));
            } 
        }

        public RubyArray Elements
        {
            get
            {
                return new RubyArray(GetIVar("@elements"));
            }
        }

        public RubyArray Switches
        {
            get
            {
                return new RubyArray(GetIVar("@switches"));
            }
        }

        public RubyArray Variables
        {
            get
            {
                return new RubyArray(GetIVar("@variables"));
            }
        }

        public RubyString WindowskinName
        {
            get
            {
                return new RubyString(GetIVar("@windowskin_name"));
            }
        }

        public RubyString TitleName
        {
            get
            {
                return new RubyString(GetIVar("@title_name"));
            } 
        }

        public RubyString GameoverName
        { 
            get
            {
                return new RubyString(GetIVar("@gameover_name"));
            }
        }

        public RubyString BattleTransition
        {
            get
            {
                return new RubyString(GetIVar("@battle_transition"));
            }
        }

        public AudioFile TitleBGM
        {
            get
            {
                return new AudioFile(GetIVar("@title_bgm"));
            }
        }

        public AudioFile BattleBGM
        {
            get
            {
                return new AudioFile(GetIVar("@battle_bgm"));
            }
        }

        public AudioFile BattleEndME
        {
            get
            {
                return new AudioFile(GetIVar("@battle_end_me"));
            }
        }

        public AudioFile GameoverME
        {
            get
            {
                return new AudioFile(GetIVar("@gameover_me"));
            }
        }

        public AudioFile CursorSE
        {
            get
            {
                return new AudioFile(GetIVar("@cursor_se"));
            }
        }

        public AudioFile BuzzerSE
        {
            get
            {
                return new AudioFile(GetIVar("@buzzer_se"));
            }
        }

        public AudioFile EquipSE
        {
            get
            {
                return new AudioFile(GetIVar("@equip_se"));
            }
        }

        public AudioFile ShopSE
        {
            get
            {
                return new AudioFile(GetIVar("@shop_se"));
            }
        }

        public AudioFile SaveSE
        {
            get
            {
                return new AudioFile(GetIVar("@save_se"));
            }
        }

        public AudioFile LoadSE
        {
            get
            {
                return new AudioFile(GetIVar("@load_se"));
            }
        }

        public AudioFile BattleStartSE
        {
            get
            {
                return new AudioFile(GetIVar("@battle_start_se"));
            }
        }

        public AudioFile EscapeSE
        {
            get
            {
                return new AudioFile(GetIVar("@escape_se"));
            }
        }

        public AudioFile ActorCollapseSE
        {
            get
            {
                return new AudioFile(GetIVar("@actor_collapse_se"));
            }
        }

        public AudioFile EnemyCollapseSE
        {
            get
            {
                return new AudioFile(GetIVar("@enemy_collapse_se"));
            }
        }

        public Words Words
        {
            get
            {
                return new Words(GetIVar("@words"));
            }
        }

        public RubyArray TestBattlers
        {
            get
            {
                return new RubyArray(GetIVar("@test_battlers"));
            }
        }

        public RubyInt TestTroopID
        {
            get
            {
                return new RubyInt(GetIVar("@test_troop_id"));
            }
        }

        public RubyInt StartMapID
        {
            get
            {
                return new RubyInt(GetIVar("@start_map_id"));
            }
        }

        public RubyInt StartX
        {
            get
            {
                return new RubyInt(GetIVar("@start_x"));
            }
        }

        public RubyInt StartY
        {
            get
            {
                return new RubyInt(GetIVar("@start_y"));
            }
        }

        public RubyString BattlebackName
        {
            get
            {
                return new RubyString(GetIVar("@battleback_name"));
            }
        }

        public RubyString BattlerName
        {
            get
            {
                return new RubyString(GetIVar("@battler_name"));
            } 
        }

        public RubyInt BattlerHue
        {
            get
            {
                return new RubyInt(GetIVar("@battler_hue"));
            }
        }

        public RubyInt EditMapID
        {
            get
            {
                return new RubyInt(GetIVar("@edit_map_id"));
            }
        }
    }
}
