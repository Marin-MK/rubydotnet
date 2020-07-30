using System;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;

namespace rubydotnet
{
    public static partial class Ruby
    {
        public delegate IntPtr ProtectedMethod(IntPtr Arg);
        public delegate IntPtr RubyMethod(IntPtr Self, IntPtr Args);
        public delegate IntPtr BlockMethod(IntPtr BlockArgs, IntPtr Self, IntPtr Args);

        public const string RubyPath = "x64-msvcrt-ruby270";
        public const string Version = "2.7.0";

        public static IntPtr True  = (IntPtr) 0x14;
        public static IntPtr False = (IntPtr) 0x00;
        public static IntPtr Nil   = (IntPtr) 0x08;
        public static IntPtr Undef = (IntPtr) 0x34;

        static bool Initialized = false;

        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Ansi)]
        public static extern IntPtr LoadLibrary([MarshalAs(UnmanagedType.LPStr)] string lpFileName);

        public static void Initialize()
        {
            if (Initialized) return;
            string pwd = System.IO.Directory.GetCurrentDirectory();
            IntPtr libgmp = LoadLibrary("./lib/libgmp-10.dll");
            IntPtr libssp = LoadLibrary("./lib/libssp-0.dll");
            IntPtr libwinpthread = LoadLibrary("./lib/libwinpthread-1.dll");
            IntPtr ruby = LoadLibrary("./lib/x64-msvcrt-ruby270.dll");
            if (ruby == IntPtr.Zero) throw new Exception("Could not find Ruby at 'lib/x64-msvcrt-ruby270.dll'.");
            if (libgmp == IntPtr.Zero) throw new Exception("Could not find libgmp at 'lib/libgmp-10.dll'.");
            if (libssp == IntPtr.Zero) throw new Exception("Could not find libssp at 'lib/libssp-0.dll'.");
            if (libwinpthread == IntPtr.Zero) throw new Exception("Could not find libwinpthread at 'lib/libwinpthread-1.dll'.");
            ruby_init();
            Initialized = true;
        }

        public static void Pin(IntPtr Object)
        {
            if (GetGlobal("$__rdncache__") == Nil) SetGlobal("$__rdncache__", Array.Create());
            if (Funcall(GetGlobal("$__rdncache__"), "include?", Object) == False) Funcall(GetGlobal("$__rdncache__"), "push", Object);
        }

        public static void Unpin(IntPtr Object)
        {
            if (GetGlobal("$__rdncache__") == Nil) return;
            if (Funcall(GetGlobal("$__rdncache__"), "include?", Object) == True) Funcall(GetGlobal("$__rdncache__"), "delete", Object);
        }

        /// <summary>
        /// Removes any non-standard classes and clears all non-standard global variables.
        /// Does not remove aliases.
        /// </summary>
        public static void Reset()
        {
            IntPtr gvars = rb_f_global_variables();
            Pin(gvars);
            string[] gvkeep = new string[]
            {
                "$VERBOSE", "$-v", "$-w", "$stdin", "$stdout", "$&", "$`", "$'", "$+", "$=", "$KCODE", "$-K", "$>", "$-W",
                "$DEBUG", "$stderr", "$0", "$PROGRAM_NAME", "$,", "$/", "$-0", "$\\", "$FILENAME", "$.", "$<", "$SAFE", "$*",
                "$-i", "$:", "$-I", "$LOAD_PATH", "$\"", "$LOADED_FEATURES", "$_", "$~", "$!", "$@", "$;", "$-F", "$?", "$$", "$-d"
            };
            long gvarlen = Array.Length(gvars);
            for (int i = 0; i < gvarlen; i++)
            {
                string sym = Symbol.FromPtr(Array.Get(gvars, i));
                if (!gvkeep.Contains(sym))
                {
                    rb_gv_set(sym, Nil);
                }
            }
            Unpin(gvars);
            string[] ckeep = new string[]
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
            IntPtr consts = Funcall(Object.Class, "constants");
            Pin(consts);
            long constslen = Array.Length(consts);
            for (int i = 0; i < constslen; i++)
            {
                string constant = Symbol.FromPtr(Array.Get(consts, i));
                if (!ckeep.Contains(constant))
                {
                    Funcall(Object.Class, "remove_const", Array.Get(consts, i));
                }
            }
            Unpin(consts);
        }

        public static void Raise(ErrorType ErrorType, string Message = null)
        {
            rb_raise(GetConst(Object.Class, ErrorType.ToString()), Message ?? "");
        }

        #region Code Evaluation
        public static IntPtr Eval(string Code, bool PrintError = true, bool ThrowError = true)
        {
            IntPtr State = IntPtr.Zero;
            IntPtr ptr = rb_eval_string_protect(Code, ref State);
            if (State != IntPtr.Zero) RaiseException(PrintError, ThrowError);
            return ptr;
        }

        public static void Load(string File)
        {
            Require(File);
        }
        public static void Require(string File)
        {
            SafeRuby(delegate (IntPtr Arg)
            {
                rb_require(File);
                return IntPtr.Zero;
            });
        }

        static IntPtr SafeRuby(ProtectedMethod Method)
        {
            IntPtr State = IntPtr.Zero;
            IntPtr Result = rb_protect(Method, Nil, ref State);
            if (State != IntPtr.Zero) RaiseException();
            return Result;
        }

        static void RaiseException(bool PrintError = true, bool ThrowError = true)
        {
            IntPtr Err = rb_gv_get("$!");
            string type = String.FromPtr(Funcall(Funcall(Err, "class"), "to_s"));
            string msg = type + ": " + String.FromPtr(Funcall(Err, "to_s"));
            IntPtr Backtrace = Funcall(Err, "backtrace");
            long length = Array.Length(Backtrace);
            if (length > 0) msg += "\n\n";
            for (int i = 0; i < length; i++)
            {
                string line = String.FromPtr(Array.Get(Backtrace, i));
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
                if (i != length - 1) msg += "\n";
            }
            msg = msg[0].ToString().ToUpper() + msg.Substring(1);
            if (PrintError) Console.WriteLine(msg);
            if (ThrowError) throw new Exception(msg);
        }

        public static IntPtr Funcall(IntPtr Object, string Method, params IntPtr[] Args)
        {
            return rb_funcallv(Object, rb_intern(Method), Args.Length, Args);
        }
        #endregion

        #region Variable Shortcuts
        public static IntPtr GetGlobal(string Name)
        {
            return rb_gv_get(Name);
        }
        public static void SetGlobal(string Name, IntPtr Value)
        {
            rb_gv_set(Name, Value);
        }

        public static IntPtr GetIVar(IntPtr Object, string Name)
        {
            return rb_ivar_get(Object, rb_intern(Name));
        }
        public static IntPtr SetIVar(IntPtr Object, string Name, IntPtr Value)
        {
            return rb_ivar_set(Object, rb_intern(Name), Value);
        }
        #endregion

        #region Type Validation
        public static void Expect(IntPtr Object, params string[] Class)
        {
            if (!Is(Object, Class))
            {
                string classstr = "";
                for (int i = 0; i < Class.Length; i++)
                {
                    classstr += Class[i];
                    if (i == Class.Length - 2) classstr += " or ";
                    else if (i < Class.Length - 2) classstr += ", ";
                }
                Raise(ErrorType.ArgumentError, $"expected {classstr}, but got {GetClassName(Object)} instead.");
            }
        }

        public static bool Is(IntPtr Object, params string[] Class)
        {
            for (int i = 0; i < Class.Length; i++)
            {
                if (GetClassName(Object) == Class[i]) return true;
            }
            return false;
        }

        public static IntPtr GetClass(IntPtr Object)
        {
            return Funcall(Object, "class");
        }

        public static string GetClassName(IntPtr Object)
        {
            return String.FromPtr(Funcall(GetClass(Object), "to_s"));
        }

        public static bool IVarIs(IntPtr Object, string Variable, params string[] Class)
        {
            return Is(GetIVar(Object, Variable), Class);
        }
        #endregion

        #region Constants
        public static IntPtr GetConst(IntPtr Object, string Name)
        {
            return Funcall(Object, "const_get", String.ToPtr(Name));
        }

        public static void SetConst(IntPtr Object, string Name, IntPtr Value)
        {
            Funcall(Object, "const_set", String.ToPtr(Name), Value);
        }
        #endregion

        #region Block Methods
        public static void RequireBlock()
        {
            rb_need_block();
        }
        public static bool HasBlock()
        {
            return rb_block_given_p();
        }
        public static IntPtr GetBlock()
        {
            return rb_block_proc();
        }
        public static IntPtr Yield(IntPtr Value)
        {
            return rb_yield(Value);
        }
        #endregion

        public enum ErrorType
        {
            RuntimeError,
            ArgumentError,
            TypeError,
            SystemExit
        }

        #region Import
        #region Utility
        [DllImport(RubyPath)]
        static extern void ruby_init();

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

        [DllImport(RubyPath)]
        static extern void rb_require(string File);
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
        static extern void rb_define_method(IntPtr Object, string Name, [MarshalAs(UnmanagedType.FunctionPtr)] RubyMethod Function, int Argc);

        [DllImport(RubyPath)]
        static extern void rb_define_singleton_method(IntPtr Object, string Name, [MarshalAs(UnmanagedType.FunctionPtr)] RubyMethod Function, int Argc);

        [DllImport(RubyPath)]
        static extern IntPtr rb_define_const(IntPtr Klass, string Name, IntPtr Object);
        #endregion

        #region Method
        [DllImport(RubyPath)]
        static extern IntPtr rb_funcallv(IntPtr Object, IntPtr Function, int Argc, params IntPtr[] Argv);

        [DllImport(RubyPath)]
        static extern IntPtr rb_block_call(IntPtr Object, IntPtr Function, int Argc, IntPtr[] Argv, BlockMethod Block, IntPtr data2);

        [DllImport(RubyPath)]
        static extern void rb_need_block();

        [DllImport(RubyPath)]
        static extern bool rb_block_given_p();

        [DllImport(RubyPath)]
        static extern IntPtr rb_block_proc();

        [DllImport(RubyPath)]
        static extern IntPtr rb_yield(IntPtr Value);

        [DllImport(RubyPath)]
        static extern IntPtr rb_ivar_get(IntPtr Object, IntPtr ID);

        [DllImport(RubyPath)]
        static extern IntPtr rb_ivar_set(IntPtr Object, IntPtr ID, IntPtr Value);
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
