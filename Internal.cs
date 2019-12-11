using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace RubyDotNET
{
    public static class Internal
    {
        public static Dictionary<IntPtr, Klass> Klasses = new Dictionary<IntPtr, Klass>();

        public static Class cObject;
        public static Class cFile;
        public static Class cArray;
        public static Class cHash;

        public static void Initialize()
        {
            ruby_init();
            cObject = RubyObject.CreateClass(Eval("Object"));
            cFile = RubyFile.CreateClass(Eval("File"));
            cArray = RubyArray.CreateClass(Eval("Array"));
            cHash = RubyHash.CreateClass(Eval("Hash"));
        }

        public static Klass GetKlass(string KlassName)
        {
            foreach (KeyValuePair<IntPtr, Klass> kvp in Klasses)
            {
                if (kvp.Value.KlassName == KlassName) return kvp.Value;
            }
            return null;
        }

        public static void SetGlobalVariable(string VariableName, IntPtr Value)
        {
            rb_gv_set(VariableName, Value);
        }

        public static IntPtr GetGlobalVariable(string VariableName)
        {
            return rb_gv_get(VariableName);
        }

        public static IntPtr Eval(string Code)
        {
            IntPtr state = IntPtr.Zero;
            IntPtr Value = rb_eval_string_protect(Code, state);
            if (state != IntPtr.Zero)
            {
                throw new Exception("Error.");
            }
            return Value;
        }

        public static void PrintObject(RubyObject obj)
        {
            SetGlobalVariable("$_temp", obj.Pointer);
            Eval("p $_temp");
            SetGlobalVariable("$_temp", QNil);
        }

        public static void DoStuff()
        {
            Module mRPG = new Module("RPG");
            Class cTable = Table.CreateClass();
            Class cTone = Tone.CreateClass();
            Class cColor = Color.CreateClass();
            Class cAudioFile = AudioFile.CreateClass();
            Class cMap = Map.CreateClass();
            Class cEventCommand = new Class("EventCommand", null, mRPG);
            Class cMoveCommand = new Class("MoveCommand", null, mRPG);
            Class cMoveRoute = new Class("MoveRoute", null, mRPG);
            Class cEvent = new Class("Event", null, mRPG);
            Class cPage = new Class("Page", null, cEvent);
            Class cCondition = new Class("Condition", null, cPage);
            Class cGraphic = new Class("Graphic", null, cPage);

            RubyFile f = RubyFile.Open("D:\\Desktop\\Main Essentials\\Data\\Map002.rxdata", "rb");
            RubyString RawData = f.Read();
            f.Close();

            Map map = RubyMarshal.Load<Map>(RawData);
            
            RubyArray keys = map.Events.Keys;

            for (int i = 0; i < keys.Length; i++)
            {
                Event e = map.Events[keys[i]].Convert<Event>();
                int pagecount = e.Pages.Length;
                Console.WriteLine($"EventID: {e.ID}");
                Console.WriteLine($"EventName: {e.Name}");
                Console.WriteLine($"EventX: {e.X}");
                Console.WriteLine($"EventY: {e.Y}");
                for (int j = 0; j < e.Pages.Length; j++)
                {
                    Page p = e.Pages[j].Convert<Page>();
                    Console.WriteLine($"Page{j}Page: {p}");
                    Console.WriteLine($"Page{j}MoveType: {p.MoveType}");
                    Console.WriteLine($"Page{j}MoveSpeed: {p.MoveSpeed}");
                    Console.WriteLine($"Page{j}MoveFrequency: {p.MoveFrequency}");
                    Console.WriteLine($"Page{j}WalkAnime: {p.WalkAnime}");
                    Console.WriteLine($"Page{j}StepAnime: {p.StepAnime}");
                    Console.WriteLine($"Page{j}DirectionFix: {p.DirectionFix}");
                    Console.WriteLine($"Page{j}Through: {p.Through}");
                    Console.WriteLine($"Page{j}AlwaysOnTop: {p.AlwaysOnTop}");
                    Console.WriteLine($"Page{j}Trigger: {p.Trigger}");
                    Condition c = p.Condition;
                    Console.WriteLine($"Page{j}ConditionSwitch1: {c.Switch1ID}-{c.Switch1Valid}");
                    Console.WriteLine($"Page{j}ConditionSwitch2: {c.Switch2ID}-{c.Switch2Valid}");
                    Console.WriteLine($"Page{j}ConditionVariable: {c.VariableID}-{c.VariableValue}-{c.VariableValid}");
                    Console.WriteLine($"Page{j}ConditionSelfSwitch: {c.SelfSwitchChar}-{c.SelfSwitchValid}");
                    Graphic g = p.Graphic;
                    Console.WriteLine($"Page{j}GraphicTileID: {g.TileID}");
                    Console.WriteLine($"Page{j}GraphicCharacterName: {g.CharacterName}");
                    Console.WriteLine($"Page{j}GraphicCharacterHue: {g.CharacterHue}");
                    Console.WriteLine($"Page{j}GraphicDirection: {g.Direction}");
                    Console.WriteLine($"Page{j}GraphicPattern: {g.Pattern}");
                    Console.WriteLine($"Page{j}GraphicOpacity: {g.Opacity}");
                    Console.WriteLine($"Page{j}GraphicBlendType: {g.BlendType}");
                    MoveRoute mr = p.MoveRoute;
                    Console.WriteLine($"Page{j}MoveRouteRepeat: {mr.Repeat}");
                    Console.WriteLine($"Page{j}MoveRouteSkippable: {mr.Skippable}");
                    for (int k = 0; k < mr.List.Length; k++)
                    {
                        MoveCommand mc = mr.List[k].Convert<MoveCommand>();
                        Console.WriteLine($"Page{j}MoveRoute{k}Code : {mc.Code}-{mc.Parameters}");
                    }
                    for (int k = 0; k < p.List.Length; k++)
                    {
                        EventCommand ec = p.List[k].Convert<EventCommand>();
                        Console.WriteLine($"Page{j}EventCommand{k} : {ec.Code}-{ec.Indent}-{ec.Parameters}");
                    }
                }
            }
        }

        public static IntPtr LONG2NUM(long v)
        {
            if (RB_FIXABLE(v))
                return RB_INT2FIX(v);
            else
                return rb_int2big(v);
        }

        public static IntPtr RB_INT2FIX(long v)
        {
#pragma warning disable CS0675 // Bitwise-or operator used on a sign-extended operand
            return (IntPtr) ((v << 1) | RUBY_FIXNUM_FLAG);
#pragma warning restore CS0675 // Bitwise-or operator used on a sign-extended operand
        }

        public static long NUM2LONG(IntPtr Value)
        {
            long ptr = Value.ToInt64();
            if ((ptr & 1) == 1)
            {
                return ptr >> 1;
            }
            return rb_num2long((IntPtr) ptr);
        }

        public static bool RB_FIXABLE(long f)
        {
            return RB_POSFIXABLE(f) && RB_NEGFIXABLE(f);
        }

        public static bool RB_POSFIXABLE(long f)
        {
            return f < RUBY_FIXNUM_MAX + 1;
        }

        public static bool RB_NEGFIXABLE(long f)
        {
            return f >= RUBY_FIXNUM_MIN;
        }

        public static IntPtr QTrue = (IntPtr) 20;
        public static IntPtr QFalse = (IntPtr) 0;
        public static IntPtr QNil = (IntPtr) 8;
        public static IntPtr State = Marshal.AllocHGlobal(sizeof(int));

        public const string RubyPath = "libruby.dll";

        #region Constants
        public static int LONG_MAX = 2147483647;
        public static int LONG_MIN = -LONG_MAX - 1;

        public static int RUBY_FIXNUM_FLAG = 0x01;
        public static int RUBY_FIXNUM_MAX = LONG_MAX >> 1;
        public static int RUBY_FIXNUM_MIN = LONG_MIN >> 1;

        public static int RUBY_FL_USHIFT = 12;

        public static int RUBY_FL_USER1 = 1 << (RUBY_FL_USHIFT + 1);
        public static int RUBY_FL_USER3 = 1 << (RUBY_FL_USHIFT + 3);
        public static int RUBY_FL_USER4 = 1 << (RUBY_FL_USHIFT + 4);

        public static int RARRAY_EMBED_FLAG = RUBY_FL_USER1;
        public static int RARRAY_EMBED_LEN_MASK = RUBY_FL_USER4 | RUBY_FL_USER3;
        public static int RARRAY_EMBED_LEN_SHIFT = RUBY_FL_USHIFT + 3;
        #endregion

        #region Other
        [DllImport(RubyPath)]
        public static extern void ruby_init();

        [DllImport(RubyPath)]
        public static extern IntPtr rb_eval_string_protect(string Code, IntPtr state);

        [DllImport(RubyPath)]
        public static extern void rb_gv_set(string Var, IntPtr Value);

        [DllImport(RubyPath)]
        public static extern IntPtr rb_gv_get(string Var);

        [DllImport(RubyPath)]
        public static extern void rb_global_variable(IntPtr Object);

        [DllImport(RubyPath)]
        public static extern void rb_define_variable(string VarName, IntPtr Object);
        #endregion

        #region Klasses & Methods
        [DllImport(RubyPath)]
        public static extern IntPtr rb_funcallv(IntPtr Object, IntPtr Function, int Argc, params IntPtr[] Argv);

        [DllImport(RubyPath)]
        public static extern IntPtr rb_intern(string Str);

        [DllImport(RubyPath)]
        public static extern IntPtr rb_define_module(string Name);

        [DllImport(RubyPath)]
        public static extern IntPtr rb_define_module_under(IntPtr Klass, string Name);

        [DllImport(RubyPath)]
        public static extern IntPtr rb_define_class(string Name, IntPtr InheritedClass);

        [DllImport(RubyPath)]
        public static extern IntPtr rb_define_class_under(IntPtr Klass, string Name, IntPtr InheritedClass);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr RubyMethod(int Argc, IntPtr[] Argv, IntPtr Self);

        [DllImport(RubyPath)]
        public static extern void rb_define_method(IntPtr Klass, string Name, [MarshalAs(UnmanagedType.FunctionPtr)]RubyMethod Function, int Argc);

        [DllImport(RubyPath)]
        public static extern void rb_define_singleton_method(IntPtr Klass, string Name, [MarshalAs(UnmanagedType.FunctionPtr)]RubyMethod Function, int Argc);

        [DllImport(RubyPath)]
        public static extern IntPtr rb_ivar_get(IntPtr Object, IntPtr ID);

        [DllImport(RubyPath)]
        public static extern IntPtr rb_ivar_set(IntPtr Object, IntPtr ID, IntPtr Value);

        [DllImport(RubyPath)]
        public static extern void rb_scan_args(int Argc, IntPtr[] Argv, string fmt, IntPtr[] Values);
        #endregion

        #region String
        [DllImport(RubyPath)]
        public static extern IntPtr rb_str_new_cstr(string String);

        [DllImport(RubyPath)]
        public static extern IntPtr rb_string_value_ptr(ref IntPtr Value);

        [DllImport(RubyPath)]
        public static extern long rb_str_strlen(IntPtr Value);
        #endregion

        #region Files & IO
        [DllImport(RubyPath)]
        public static extern IntPtr rb_file_open(string file, string mode);

        [DllImport(RubyPath)]
        public static extern IntPtr rb_io_close(IntPtr stream);
        #endregion

        #region Marshal
        [DllImport(RubyPath)]
        public static extern IntPtr rb_marshal_load(IntPtr Data);

        [DllImport(RubyPath)]
        public static extern IntPtr rb_marshal_dump(IntPtr Data, IntPtr Val);
        #endregion

        #region Numeric
        [DllImport(RubyPath)]
        public static extern long rb_num2long(IntPtr Value);

        [DllImport(RubyPath)]
        public static extern IntPtr rb_int2big(long Value);
        #endregion

        #region Array
        [DllImport(RubyPath)]
        public static extern IntPtr rb_ary_entry(IntPtr Object, int Index);

        [DllImport(RubyPath)]
        public static extern IntPtr rb_ary_new();

        [DllImport(RubyPath, EntryPoint = "rb_ary_length")]
        public static extern IntPtr rb_ary_length(IntPtr Object);

        [DllImport(RubyPath)]
        public static extern void rb_ary_store(IntPtr Object, long Index, IntPtr Value);

        [DllImport(RubyPath)]
        public static extern IntPtr rb_ary_push(IntPtr Object, IntPtr Value);

        [DllImport(RubyPath)]
        public static extern IntPtr rb_ary_splice(IntPtr Object, long beg, long len, IntPtr rptr, long rlen);

        [DllImport(RubyPath)]
        public static extern IntPtr rb_ary_delete_at(IntPtr Object, long Index);
        #endregion

        #region Hash
        [DllImport(RubyPath)]
        public static extern IntPtr rb_hash_new();

        [DllImport(RubyPath)]
        public static extern IntPtr rb_hash_aref(IntPtr Object, IntPtr Key);

        [DllImport(RubyPath)]
        public static extern IntPtr rb_hash_aset(IntPtr Object, IntPtr Key, IntPtr Value);

        [DllImport(RubyPath)]
        public static extern IntPtr rb_hash_size(IntPtr Object);

        [DllImport(RubyPath)]
        public static extern IntPtr rb_hash_delete(IntPtr Object, IntPtr Key);
        #endregion
    }

    public struct RBasic
    {
        public IntPtr flags;
        public IntPtr klass;
    }

    public struct RArray
    {
        public RBasic basic;
        public RArrayAs _as;
    }

    public struct RArrayAs
    {
        public IntPtr ary;
        public RArrayAsHeap heap;
    }

    public struct RArrayAsHeap
    {
        public long len;
        public IntPtr ptr;
        public RArrayAsHeapAux aux;
    }

    public struct RArrayAsHeapAux
    {
        public long capa;
        public IntPtr shared_root;
    }
}
