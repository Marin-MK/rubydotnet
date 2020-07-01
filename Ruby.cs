using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace rubydotnet
{
    public static partial class Ruby
    {
        public delegate Object Method(Object self, Array Args);
        public delegate Object Proc(Object Args);
        delegate IntPtr InternalMethod(IntPtr self, IntPtr args);
        delegate IntPtr ProtectedMethod(IntPtr arg);
        delegate IntPtr BlockMethod(IntPtr block_arg, IntPtr data, int Argc, IntPtr[] Argv);

        public const string RubyPath = "x64-msvcrt-ruby270";
        public const string Version = "2.7.0";

        public static NilClass Nil;
        public static TrueClass True;
        public static FalseClass False;

        static bool Initialized = false;

        public static void Initialize()
        {
            if (Initialized) return;
            ruby_init();
            Nil = new NilClass();
            True = new TrueClass();
            False = new FalseClass();
            Object.Class = Eval("Object").Convert<Class>();
            Initialized = true;
        }

        /// <summary>
        /// Removes any non-standard classes and clears all non-standard global variables.
        /// Does not remove aliases.
        /// </summary>
        public static void Reset()
        {
            foreach (KeyValuePair<Class, Type> kvp in Class.CustomClasses)
            {
                FieldInfo info = kvp.Value.GetField("KlassName", BindingFlags.Public | System.Reflection.BindingFlags.Static);
                string klassname = (string) info.GetValue(null);
                Object parent = Object.Class;
                if (klassname.Contains("::"))
                {
                    string[] klasses = klassname.Split("::");
                    string pklass = "";
                    for (int i = 0; i < klasses.Length - 1; i++)
                    {
                        pklass += klasses[i];
                        if (i != klasses.Length - 2) pklass += "::";
                    }
                    parent = Object.GetKlass(pklass);
                    klassname = klasses.Last();
                }
                parent.Funcall("instance_eval", delegate (Ruby.Object Arg)
                {
                    return Arg.Funcall("remove_const", new Ruby.Symbol(klassname));
                });
            }
            foreach (KeyValuePair<Module, Type> kvp in Module.CustomModules)
            {
                FieldInfo info = kvp.Value.GetField("KlassName", BindingFlags.Public | System.Reflection.BindingFlags.Static);
                string klassname = (string) info.GetValue(null);
                Object parent = Object.Class;
                if (klassname.Contains("::"))
                {
                    string[] klasses = klassname.Split("::");
                    string pklass = "";
                    for (int i = 0; i < klasses.Length - 1; i++)
                    {
                        pklass += klasses[i];
                        if (i != klasses.Length - 2) pklass += "::";
                    }
                    parent = Object.GetKlass(pklass);
                    klassname = klasses.Last();
                }
                parent.Funcall("instance_eval", delegate (Ruby.Object Arg)
                {
                    return Arg.Funcall("remove_const", new Ruby.Symbol(klassname));
                });
            }
            Array gvars = new Array(rb_f_global_variables());
            gvars.Pin();
            Symbol[] gvkeep = new Symbol[]
            {
                "$VERBOSE", "$-v", "$-w", "$stdin", "$stdout", "$&", "$`", "$'", "$+", "$=", "$KCODE", "$-K", "$>", "$-W",
                "$DEBUG", "$stderr", "$0", "$PROGRAM_NAME", "$,", "$/", "$-0", "$\\", "$FILENAME", "$.", "$<", "$SAFE", "$*",
                "$-i", "$:", "$-I", "$LOAD_PATH", "$\"", "$LOADED_FEATURES", "$_", "$~", "$!", "$@", "$;", "$-F", "$?", "$$", "$-d"
            };
            for (int i = 0; i < gvars.Length; i++)
            {
                if (!gvkeep.Contains(gvars[i].Convert<Symbol>()))
                {
                    rb_gv_set(gvars[i].Convert<Symbol>().ToString().Substring(1), QNil);
                }
            }
            gvars.Unpin();
            Symbol[] ckeep = new Symbol[]
            {
                "SystemExit", "IO", "SignalException", "Interrupt", "StandardError", "TypeError", "ArgumentError", "IndexError",
                "KeyError", "RangeError", "ScriptError", "SyntaxError", "LoadError", "Random", "NotImplementedError", "NameError",
                "NoMethodError", "RuntimeError", "FrozenError", "SecurityError", "String", "NoMemoryError", "EncodingError", "NilClass",
                "SystemCallError", "Float", "NIL", "Errno", "Warning", "NoMatchingPatternError", "Array", "Hash", "Integer", "Proc",
                "Signal", "LocalJumpError", "SystemStackError", "Method", "UnboundMethod", "Binding", "Data", "TrueClass", "TRUE",
                "FalseClass", "FALSE", "Encoding", "STDIN", "STDOUT", "UncaughtThrowError", "STDERR", "Math", "Fiber", "FiberError",
                "ARGF", "Rational", "Comparable", "FileTest", "File", "Enumerable", "ZeroDivisionError", "FloatDomainError", "Numeric",
                "Complex", "Enumerator", "RUBY_VERSION", "RUBY_RELEASE_DATE", "RUBY_PLATFORM", "RUBY_PATCHLEVEL", "RUBY_REVISION",
                "RUBY_COPYRIGHT", "RUBY_ENGINE", "RUBY_ENGINE_VERSION", "GC", "ENV", "Fixnum", "ObjectSpace", "Struct", "StopIteration",
                "RegexpError", "RubyVM", "Regexp", "Thread", "Bignum", "TOPLEVEL_BINDING", "MatchData", "TracePoint", "Dir", "ARGV",
                "ThreadGroup", "ThreadError", "Mutex", "Queue", "ClosedQueueError", "SizedQueue", "Marshal", "ConditionVariable", "Time",
                "UnicodeNormalize", "Range", "BasicObject", "Object", "Module", "Class", "EOFError", "Kernel", "Symbol", "Exception",
                "IOError", "Process"
            };
            Array consts = Object.Class.Funcall("constants").Convert<Array>();
            consts.Pin();
            for (int i = 0; i < consts.Length; i++)
            {
                if (!ckeep.Contains(consts[i].Convert<Symbol>()))
                {
                    Object.Class.Funcall("remove_const", consts[i]);
                }
            }
            consts.Unpin();
            Class.CustomClasses.Clear();
            Module.CustomModules.Clear();
        }

        public static void Raise(ErrorType ErrorType, string Message = null)
        {
            rb_raise(Object.GetKlass(ErrorType.ToString()).Pointer, Message ?? "");
        }

        public static void SetGlobal(string Name, Object obj)
        {
            rb_gv_set(Name, obj.Pointer);
        }

        public static Object Eval(string Code, bool PrintError = true, bool ThrowError = true)
        {
            IntPtr State = IntPtr.Zero;
            IntPtr Result = rb_eval_string_protect(Code, ref State);
            if (State != IntPtr.Zero) RaiseException(PrintError, ThrowError);
            return new Object(Result, false);
        }

        static IntPtr SafeRuby(ProtectedMethod Method)
        {
            return SafeRuby(Method, IntPtr.Zero);
        }
        static IntPtr SafeRuby(ProtectedMethod Method, IntPtr Argument)
        {
            IntPtr State = IntPtr.Zero;
            IntPtr Result = rb_protect(Method, Argument, ref State);
            if (State != IntPtr.Zero) RaiseException();
            return Result;
        }

        static void RaiseException(bool PrintError = true, bool ThrowError = true)
        {
            string type = NUM2STR(rb_eval_string("$!.class.to_s"));
            string msg = type + ": " + Eval("$!.to_s").Convert<String>();
            Array stack = Eval("$!.backtrace").Convert<Array>();
            if (stack.Length > 0) msg += "\n\n";
            for (int i = 0; i < stack.Length; i++)
            {
                string line = stack[i].ToString();
                string[] colons = line.Split(':');
                int? linenum = null;
                if (System.Text.RegularExpressions.Regex.IsMatch(colons[1], @"/\d+/"))
                {
                    linenum = Convert.ToInt32(colons[1]);
                }
                msg += "from ";
                for (int j = linenum == null ? 1 : 2; j < colons.Length; j++)
                {
                    if (colons[j].Length > 3 && colons[j][0] == 'i' && colons[j][1] == 'n' && colons[j][2] == ' ')
                        colons[j] = colons[j].Replace("in ", "");
                    msg += colons[j];
                    if (j != colons.Length - 1) msg += ":";
                }
                if (linenum != null) msg += " line " + linenum.ToString();
                if (i != stack.Length - 1) msg += "\n";
            }
            msg = msg[0].ToString().ToUpper() + msg.Substring(1);
            if (PrintError) Console.WriteLine(msg);
            if (ThrowError) throw new Exception(msg);
        }

        static IntPtr LONG2NUM(long Value)
        {
            if (RB_FIXABLE(Value)) return RB_INT2FIX(Value);
            else return rb_int2big(Value);
        }
        static long NUM2LONG(IntPtr Value)
        {
            if (RB_FIXNUM_P(Value)) return (int) Value >> 1;
            return rb_num2ll(Value);
        }
        static long NUM2LONG(Integer Value)
        {
            return NUM2LONG(Value.Pointer);
        }

        static IntPtr INT2NUM(int Value)
        {
            return RB_INT2FIX(Value);
        }
        static IntPtr INT2NUM(long Value)
        {
            if (RB_FIXABLE(Value)) return RB_INT2FIX(Value);
            else throw new Exception("Cannot cast Int64 into Fixnum");
        }
        static int NUM2INT(IntPtr Value)
        {
            if (RB_FIXNUM_P(Value)) return (int) Value >> 1;
            else throw new Exception("Cannot cast Ruby Int64 into a Int32");
        }
        static int NUM2INT(Integer Value)
        {
            return NUM2INT(Value.Pointer);
        }

        static IntPtr STR2NUM(string Value)
        {
            return rb_str_new(Value, Value.Length);
        }
        static string NUM2STR(IntPtr Value)
        {
            int len = NUM2INT(rb_funcallv(Value, rb_intern("bytesize"), 0));
            return System.Runtime.InteropServices.Marshal.PtrToStringUTF8(rb_string_value_ptr(ref Value), len);
        }
        static string NUM2STR(String Value)
        {
            return NUM2STR(Value.Pointer);
        }

        static IntPtr DBL2NUM(double Value)
        {
            return rb_float_new(Value);
        }
        static double NUM2DBL(IntPtr Value)
        {
            return rb_num2dbl(Value);
        }
        static double NUM2DBL(Float Value)
        {
            return NUM2DBL(Value.Pointer);
        }

        public static long ToInt64(this DateTime time)
        {
            return (long) Math.Round(time.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds);
        }

        #region Internal utility
        static bool RB_FIXABLE(long f)
        {
            return RB_POSFIXABLE(f) && RB_NEGFIXABLE(f);
        }

        static bool RB_POSFIXABLE(long f)
        {
            return f < RUBY_FIXNUM_MAX + 1;
        }

        static bool RB_NEGFIXABLE(long f)
        {
            return f >= RUBY_FIXNUM_MIN;
        }

        static bool RB_FIXNUM_P(IntPtr v)
        {
            return (((int) (long) (v)) & FIXNUM_FLAG) == 1;
        }

        static IntPtr RB_INT2FIX(long v)
        {
#pragma warning disable CS0675 // Bitwise-or operator used on a sign-extended operand
            return (IntPtr) ((v << 1) | RUBY_FIXNUM_FLAG);
#pragma warning restore CS0675 // Bitwise-or operator used on a sign-extended operand
        }

        static IntPtr QTrue  = (IntPtr) 0x14;
        static IntPtr QFalse = (IntPtr) 0x00;
        static IntPtr QNil   = (IntPtr) 0x08;
        static IntPtr QUndef = (IntPtr) 0x34;

        static int LONG_MAX = 2147483647;
        static int LONG_MIN = -LONG_MAX - 1;

        static int RUBY_FIXNUM_FLAG = 0x01;
        static int RUBY_FIXNUM_MAX = LONG_MAX >> 1;
        static int RUBY_FIXNUM_MIN = LONG_MIN >> 1;

        const int FIXNUM_FLAG = 0x01;
        const int FLONUM_MASK = 0x03;
        const int FLONUM_FLAG = 0x02;
        const int RUBY_SYMBOL_FLAG = 0x0c;
        const int IMMEDIATE_MASK = 0x07;
        const int RUBY_SPECIAL_SHIFT = 8;
        const int SYMBOL_FLAG = 0x0c;
        const int RUBY_T_MASK = 0x1f;

        public enum ErrorType
        {
            RuntimeError,
            ArgumentError,
            TypeError
        }
        #endregion

        #region Import
        #region Utility
        [DllImport(RubyPath)]
        static extern void ruby_init();

        [DllImport(RubyPath)]
        static extern IntPtr rb_eval_string(string Code);

        [DllImport(RubyPath)]
        static extern IntPtr rb_eval_string_protect(string Code, ref IntPtr State);

        [DllImport(RubyPath)]
        static extern IntPtr rb_protect(ProtectedMethod Method, IntPtr Argument, ref IntPtr State);

        [DllImport(RubyPath)]
        public static extern IntPtr rb_intern(string Str);

        [DllImport(RubyPath)]
        static extern IntPtr rb_str_intern(IntPtr Str);

        [DllImport(RubyPath)]
        static extern void rb_gv_set(string Var, IntPtr Value);

        [DllImport(RubyPath)]
        static extern IntPtr rb_gv_get(string Var);

        [DllImport(RubyPath)]
        static extern IntPtr rb_f_global_variables();

        [DllImport(RubyPath)]
        static extern void rb_raise(IntPtr Class, string Message);
        #endregion

        #region Class & Module
        [DllImport(RubyPath)]
        static extern IntPtr rb_define_class(string Name, IntPtr InheritedClass);

        [DllImport(RubyPath)]
        static extern IntPtr rb_define_class_under(IntPtr UnderKlass, string Name, IntPtr InheritedClass);

        [DllImport(RubyPath)]
        static extern IntPtr rb_define_module(string Name);

        [DllImport(RubyPath)]
        static extern IntPtr rb_define_module_under(IntPtr UnderKlass, string Name);

        [DllImport(RubyPath)]
        static extern void rb_define_method(IntPtr Object, string Name, [MarshalAs(UnmanagedType.FunctionPtr)] InternalMethod Function, int Argc);

        [DllImport(RubyPath)]
        static extern void rb_define_singleton_method(IntPtr Object, string Name, [MarshalAs(UnmanagedType.FunctionPtr)] InternalMethod Function, int Argc);
        #endregion

        #region Method
        [DllImport(RubyPath)]
        static extern IntPtr rb_funcallv(IntPtr Object, IntPtr Function, int Argc, params IntPtr[] Argv);

        [DllImport(RubyPath)]
        static extern IntPtr rb_block_call(IntPtr Object, IntPtr Function, int Argc, IntPtr[] Argv, BlockMethod Block, IntPtr data2);

        [DllImport(RubyPath)]
        static extern IntPtr rb_ivar_get(IntPtr Object, IntPtr ID);

        [DllImport(RubyPath)]
        static extern IntPtr rb_ivar_set(IntPtr Object, IntPtr ID, IntPtr Value);
        #endregion

        #region String
        [DllImport(RubyPath)]
        static extern IntPtr rb_str_new_cstr(string String);

        [DllImport(RubyPath)]
        static extern IntPtr rb_str_new(string String, long Length);

        [DllImport(RubyPath)]
        static extern long rb_str_strlen(IntPtr Object);

        [DllImport(RubyPath)]
        static extern IntPtr rb_string_value_ptr(ref IntPtr Object);
        #endregion

        #region Numeric
        [DllImport(RubyPath)]
        static extern IntPtr rb_int2big(long Value);

        [DllImport(RubyPath)]
        static extern long rb_num2ll(IntPtr Value);

        [DllImport(RubyPath)]
        static extern IntPtr rb_float_new(double Value);

        [DllImport(RubyPath)]
        static extern double rb_num2dbl(IntPtr Value);
        #endregion

        #region Array
        [DllImport(RubyPath)]
        static extern IntPtr rb_ary_new();

        [DllImport(RubyPath)]
        static extern IntPtr rb_ary_entry(IntPtr Object, int Index);
        
        [DllImport(RubyPath)]
        static extern void rb_ary_store(IntPtr Object, long Index, IntPtr Value);
        #endregion

        #region Hash
        [DllImport(RubyPath)]
        static extern IntPtr rb_hash_new();

        [DllImport(RubyPath)]
        static extern IntPtr rb_hash_keys(IntPtr Object);

        [DllImport(RubyPath)]
        static extern IntPtr rb_hash_values(IntPtr Object);

        [DllImport(RubyPath)]
        static extern IntPtr rb_hash_aref(IntPtr Object, IntPtr Key);

        [DllImport(RubyPath)]
        static extern IntPtr rb_hash_aset(IntPtr Object, IntPtr Key, IntPtr Value);

        [DllImport(RubyPath)]
        static extern IntPtr rb_hash_size(IntPtr Object);
        #endregion

        #region Range
        [DllImport(RubyPath)]
        static extern IntPtr rb_range_new(IntPtr Min, IntPtr Max, int Type);

        [DllImport(RubyPath)]
        static extern int rb_range_values(IntPtr Object, ref IntPtr Min, ref IntPtr Max, ref int Type);
        #endregion

        #region Regexp
        [DllImport(RubyPath)]
        static extern IntPtr rb_reg_new_str(IntPtr Object, int Options);
        #endregion

        #region File
        [DllImport(RubyPath)]
        static extern IntPtr rb_file_open(string Filename, string Mode);
        #endregion

        #region Marshal
        [DllImport(RubyPath)]
        static extern IntPtr rb_marshal_load(IntPtr Data);

        [DllImport(RubyPath)]
        static extern IntPtr rb_marshal_dump(IntPtr Data, IntPtr Val);
        #endregion

        #region Time
        [DllImport(RubyPath)]
        static extern IntPtr rb_time_num_new(IntPtr Value, IntPtr Offset);
        #endregion
        #endregion
    }
}
