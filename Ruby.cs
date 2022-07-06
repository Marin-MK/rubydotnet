using System;
using System.IO;
using System.Linq;
using NativeLibraryLoader;

namespace rubydotnet;

public static partial class Ruby
{
    public delegate IntPtr ProtectedMethod(IntPtr Arg);
    public delegate IntPtr RubyMethod(IntPtr Self, IntPtr Args);
    public delegate IntPtr BlockMethod(IntPtr BlockArgs, IntPtr Self, IntPtr Args);

    public static IntPtr True  = (IntPtr) 0x14;
    public static IntPtr False = (IntPtr) 0x00;
    public static IntPtr Nil   = (IntPtr) 0x08;
    public static IntPtr Undef = (IntPtr) 0x34;

    static bool Initialized = false;

    public static void Initialize(PathInfo PathInfo)
    {
        if (Initialized) return;
        NativeLibrary ruby;
        Console.WriteLine("Loading Ruby...");
        PathPlatformInfo path = PathInfo.GetPlatform(Platform.Windows);
        if (NativeLibrary.Platform == Platform.Windows)
        {
            string rubypath = path.Get("ruby");
            string libgmp = path.Get("libgmp");
            string libssp = path.Get("libssp");
            string libwinpthread = path.Get("libwinpthread");
            string libz = path.Get("libz");
            ruby = NativeLibrary.Load(rubypath, libgmp, libssp, libwinpthread, libz);
        }
        else if (NativeLibrary.Platform == Platform.Linux)
        {
            string rubypath = path.Get("ruby");
            ruby = NativeLibrary.Load(rubypath);
        }
        else
        {
            throw new NativeLibrary.UnsupportedPlatformException();
        }
        Console.Write("Loaded Ruby ");
        
        ruby_sysinit = ruby.GetFunction<RB_VoidPtrPtr>("ruby_sysinit");
        ruby_init = ruby.GetFunction<Action>("ruby_init");
        rb_path2class = ruby.GetFunction<RB_PtrStr>("rb_path2class");
        rb_eval_string_protect = ruby.GetFunction<RB_PtrStrRefPtr>("rb_eval_string_protect");
        rb_protect = ruby.GetFunction<RB_PMDPtrRefPtr>("rb_protect");
        rb_intern = ruby.GetFunction<RB_PtrStr>("rb_intern");
        rb_str_intern = ruby.GetFunction<RB_PtrPtr>("rb_str_intern");
        rb_gv_set = ruby.GetFunction<RB_StrPtr>("rb_gv_set");
        rb_gv_get = ruby.GetFunction<RB_PtrStr>("rb_gv_get");
        rb_f_global_variables = ruby.GetFunction<RB_Ptr>("rb_f_global_variables");
        rb_raise = ruby.GetFunction<RB_VoidPtrStr>("rb_raise");
        rb_require = ruby.GetFunction<RB_VoidStr>("rb_require");
        rb_define_class = ruby.GetFunction<RB_PtrStrPtr>("rb_define_class");
        rb_define_class_under = ruby.GetFunction<RB_PtrPtrStrPtr>("rb_define_class_under");
        rb_define_module = ruby.GetFunction<RB_PtrStr>("rb_define_module");
        rb_define_module_under = ruby.GetFunction<RB_PtrPtrStr>("rb_define_module_under");
        rb_define_method = ruby.GetFunction<RB_VoidPtrStrRMDInt>("rb_define_method");
        rb_define_singleton_method = ruby.GetFunction<RB_VoidPtrStrRMDInt>("rb_define_singleton_method");
        rb_define_const = ruby.GetFunction<RB_PtrPtrStrPtr>("rb_define_const");
        rb_const_get = ruby.GetFunction<RB_PtrPtrPtr>("rb_const_get");
        rb_const_set = ruby.GetFunction<RB_VoidPtrPtrPtr>("rb_const_set");
        rb_funcallv = ruby.GetFunction<RB_PtrPtrPtrIntParamsPtr>("rb_funcallv");
        rb_block_call = ruby.GetFunction<RB_PtrPtrPtrIntPtrAryBMDPtr>("rb_block_call");
        rb_need_block = ruby.GetFunction<Action>("rb_need_block");
        rb_block_given_p = ruby.GetFunction<RB_Bool>("rb_block_given_p");
        rb_block_proc = ruby.GetFunction<RB_Ptr>("rb_block_proc");
        rb_yield = ruby.GetFunction<RB_PtrPtr>("rb_yield");
        rb_ivar_get = ruby.GetFunction<RB_PtrPtrPtr>("rb_ivar_get");
        rb_ivar_set = ruby.GetFunction<RB_PtrPtrPtrPtr>("rb_ivar_set");
        rb_range_new = ruby.GetFunction<RB_PtrPtrPtrInt>("rb_range_new");
        rb_range_values = ruby.GetFunction<RB_IntPtrRefPtrRefPtrRefInt>("rb_range_values");
        rb_reg_new_str = ruby.GetFunction<RB_PtrPtrInt>("rb_reg_new_str");
        rb_file_open = ruby.GetFunction<RB_PtrStrStr>("rb_file_open");
        rb_time_num_new = ruby.GetFunction<RB_PtrPtrPtr>("rb_time_num_new");
        Array.rb_ary_new = ruby.GetFunction<RB_Ptr>("rb_ary_new");
        Array.rb_ary_entry = ruby.GetFunction<RB_PtrPtrInt>("rb_ary_entry");
        Array.rb_ary_store = ruby.GetFunction<RB_VoidPtrLngPtr>("rb_ary_store");
        Float.rb_float_new = ruby.GetFunction<RB_PtrDbl>("rb_float_new");
        Float.rb_num2dbl = ruby.GetFunction<RB_DblPtr>("rb_num2dbl");
        Hash.rb_hash_new = ruby.GetFunction<RB_Ptr>("rb_hash_new");
        Hash.rb_hash_keys = ruby.GetFunction<RB_PtrPtr>("rb_hash_keys");
        Hash.rb_hash_aref = ruby.GetFunction<RB_PtrPtrPtr>("rb_hash_aref");
        Hash.rb_hash_aset = ruby.GetFunction<RB_PtrPtrPtrPtr>("rb_hash_aset");
        Hash.rb_hash_size = ruby.GetFunction<RB_PtrPtr>("rb_hash_size");
        Integer.rb_int2big = ruby.GetFunction<RB_PtrLng>("rb_int2big");
        Integer.rb_num2ll = ruby.GetFunction<RB_LngPtr>("rb_num2ll");
        String.rb_str_new = ruby.GetFunction<RB_PtrStrInt>("rb_str_new");
        String.rb_utf8_str_new_cstr = ruby.GetFunction<RB_PtrPtr>("rb_utf8_str_new_cstr");
        String.rb_string_value_ptr = ruby.GetFunction<RB_PtrRefPtr>("rb_string_value_ptr");
        Marshal.rb_marshal_load = ruby.GetFunction<RB_PtrPtr>("rb_marshal_load");
        Marshal.rb_marshal_dump = ruby.GetFunction<RB_PtrPtrPtr>("rb_marshal_dump");
        IntPtr argc = System.Runtime.InteropServices.Marshal.AllocHGlobal(sizeof(int));
        System.Runtime.InteropServices.Marshal.WriteInt32(argc, 0);
        IntPtr argv = System.Runtime.InteropServices.Marshal.AllocHGlobal(sizeof(int));
        System.Runtime.InteropServices.Marshal.WriteInt32(argv, 0);
        ruby_sysinit(argc, argv);
        ruby_init();
        Initialized = true;
        string version = String.FromPtr(rb_const_get(Object.Class, rb_intern("RUBY_VERSION")));
        Console.WriteLine($"({version})");
    }

    public static void Pin(IntPtr Object)
    {
        if (GetGlobal("$__rdncache__") == Nil) SetGlobal("$__rdncache__", Array.Create());
        if (!Array.Includes(GetGlobal("$__rdncache__"), Object)) Array.Push(GetGlobal("$__rdncache__"), Object);
    }

    public static void Unpin(IntPtr Object)
    {
        if (GetGlobal("$__rdncache__") == Nil) return;
        if (Array.Includes(GetGlobal("$__rdncache__"), Object)) Array.Delete(GetGlobal("$__rdncache__"), Object);
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

    private static void Raise(ErrorType ErrorType, string Message = null)
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

    public static bool Require(string File)
    {
        IntPtr State = IntPtr.Zero;
        IntPtr Result = rb_protect(_ =>
        {
            rb_require(File);
            return IntPtr.Zero;
        }, Nil, ref State);
        return Result == IntPtr.Zero;
    }

    public static IntPtr SafeRuby(ProtectedMethod Method)
    {
        IntPtr State = IntPtr.Zero;
        IntPtr Result = rb_protect(Method, Nil, ref State);
        if (State != IntPtr.Zero) RaiseException();
        return Result;
    }

    private static void RaiseException(bool PrintError = true, bool ThrowError = true)
    {
        string msg = FormatException();
        if (PrintError) Console.WriteLine(msg);
        if (ThrowError) throw new Exception(msg);
    }

    private static string FormatException()
    {
        Ruby.Eval("p $!");
        IntPtr Err = rb_gv_get("!");
        IntPtr ErrKlass = Funcall(Err, "class");
        IntPtr ErrKlassStr = Funcall(ErrKlass, "to_s");
        string type = String.FromPtr(ErrKlassStr);
        string currentdir = Directory.GetCurrentDirectory();
        currentdir = currentdir.Replace('\\', '/');
        string msg = type + ": ";
        IntPtr Backtrace = IntPtr.Zero;
        if (type == "SyntaxError")
        {
            string s = String.FromPtr(Funcall(Err, "message"));
            string[] colons = s.Split(':');
            msg += colons[colons.Length - 1].Trim();
            Backtrace = Array.Create(1);
            string path = "";
            for (int i = 0; i < colons.Length - 1; i++)
            {
                path += colons[i];
                if (i != colons.Length - 2) path += ":";
            }
            Array.Set(Backtrace, 0, String.ToPtr(path));
        }
        else
        {
            msg += String.FromPtr(Funcall(Err, "message")).Replace(currentdir, "");
            Backtrace = Funcall(Err, "backtrace");
        }
        long length = Array.Length(Backtrace);
        if (length > 13) length = 13;
        if (length > 0) msg += "\n\n";
        for (int i = 0; i < length; i++)
        {
            string line = String.FromPtr(Array.Get(Backtrace, i));
            line = line.Replace(currentdir, "");
            string[] colons = line.Split(':');
            msg += "from ";
            if (colons.Length > 1)
            {
                for (int j = 0; j < colons.Length; j++)
                {
                    if (colons[j].Length > 3 && colons[j][0] == 'i' && colons[j][1] == 'n' && colons[j][2] == ' ')
                        colons[j] = colons[j].Replace("in ", "");
                    if (System.Text.RegularExpressions.Regex.IsMatch(colons[j], @"^\d+$"))
                    {
                        msg += $" line {colons[j]}";
                        if (j != colons.Length - 1) msg += ": ";
                    }
                    else
                    {
                        msg += colons[j];
                        if (j != colons.Length - 1 && j > 0) msg += ":";
                    }
                }
                if (i != length - 1) msg += "\n";
            }
            else
            {
                msg += line;
            }
        }
        return msg[0].ToString().ToUpper() + msg.Substring(1);
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
        return rb_const_get(Object, rb_intern(Name));
    }

    public static void SetConst(IntPtr Object, string Name, IntPtr Value)
    {
        rb_const_set(Object, rb_intern(Name), Value);
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

    #region Function Delegates
    internal delegate void RB_VoidPtrPtr(IntPtr Ptr1, IntPtr Ptr2);
    internal delegate IntPtr RB_PtrStrRefPtr(string Str, ref IntPtr IntPtr);
    internal delegate IntPtr RB_PMDPtrRefPtr(ProtectedMethod Method, IntPtr IntPtr1, ref IntPtr IntPtr2);
    internal delegate IntPtr RB_PtrStr(string Str);
    internal delegate IntPtr RB_PtrPtr(IntPtr IntPtr);
    internal delegate void RB_StrPtr(string Str, IntPtr Ptr);
    internal delegate IntPtr RB_Ptr();
    internal delegate void RB_VoidPtrStr(IntPtr IntPtr, string String);
    internal delegate void RB_VoidStr(string Str);
    internal delegate IntPtr RB_PtrStrPtr(string Str, IntPtr IntPtr);
    internal delegate IntPtr RB_PtrPtrStrPtr(IntPtr IntPtr1, string Str, IntPtr IntPtr2);
    internal delegate IntPtr RB_PtrPtrStr(IntPtr IntPtr, string Str);
    internal delegate void RB_VoidPtrStrRMDInt(IntPtr IntPtr, string Str, RubyMethod Method, int Int);
    internal delegate IntPtr RB_PtrPtrPtrIntParamsPtr(IntPtr IntPtr1, IntPtr IntPtr2, int Int, params IntPtr[] ParamsPtr);
    internal delegate IntPtr RB_PtrPtrPtrIntPtrAryBMDPtr(IntPtr IntPtr1, IntPtr IntPtr2, int Int, IntPtr[] IntPtrAry, BlockMethod Method, IntPtr IntPtr3);
    internal delegate bool RB_Bool();
    internal delegate void RB_VoidPtrPtrPtr(IntPtr IntPtr1, IntPtr IntPtr2, IntPtr IntPtr3);
    internal delegate IntPtr RB_PtrPtrPtr(IntPtr IntPtr1, IntPtr IntPtr2);
    internal delegate IntPtr RB_PtrPtrPtrPtr(IntPtr IntPtr1, IntPtr IntPtr2, IntPtr IntPtr3);
    internal delegate IntPtr RB_PtrPtrPtrInt(IntPtr IntPtr1, IntPtr IntPtr2, int Int);
    internal delegate int RB_IntPtrRefPtrRefPtrRefInt(IntPtr IntPtr1, ref IntPtr IntPtr2, ref IntPtr IntPtr3, ref int Int);
    internal delegate IntPtr RB_PtrPtrInt(IntPtr IntPtr, int Int);
    internal delegate IntPtr RB_PtrStrStr(string Str1, string Str2);
    internal delegate void RB_VoidPtrLngPtr(IntPtr IntPtr1, long Lng, IntPtr IntPtr2);
    internal delegate IntPtr RB_PtrDbl(double Dbl);
    internal delegate double RB_DblPtr(IntPtr IntPtr);
    internal delegate IntPtr RB_PtrLng(long Lng);
    internal delegate long RB_LngPtr(IntPtr IntPtr);
    internal delegate IntPtr RB_PtrStrInt(string Str, int Int);
    internal delegate IntPtr RB_PtrRefPtr(ref IntPtr IntPtr);
    #endregion

    #region Ruby Functions
    internal static RB_VoidPtrPtr ruby_sysinit;
    internal static Action ruby_init;
    internal static RB_PtrStr rb_path2class;
    internal static RB_PtrStrRefPtr rb_eval_string_protect;
    internal static RB_PMDPtrRefPtr rb_protect;
    internal static RB_PtrStr rb_intern;
    internal static RB_PtrPtr rb_str_intern;
    internal static RB_StrPtr rb_gv_set;
    internal static RB_PtrStr rb_gv_get;
    internal static RB_Ptr rb_f_global_variables;
    internal static RB_VoidPtrStr rb_raise;
    internal static RB_VoidStr rb_require;
    internal static RB_PtrStrPtr rb_define_class;
    internal static RB_PtrPtrStrPtr rb_define_class_under;
    internal static RB_PtrStr rb_define_module;
    internal static RB_PtrPtrStr rb_define_module_under;
    internal static RB_VoidPtrStrRMDInt rb_define_method;
    internal static RB_VoidPtrStrRMDInt rb_define_singleton_method;
    internal static RB_PtrPtrStrPtr rb_define_const;
    internal static RB_PtrPtrPtr rb_const_get;
    internal static RB_VoidPtrPtrPtr rb_const_set;
    internal static RB_PtrPtrPtrIntParamsPtr rb_funcallv;
    internal static RB_PtrPtrPtrIntPtrAryBMDPtr rb_block_call;
    internal static Action rb_need_block;
    internal static RB_Bool rb_block_given_p;
    internal static RB_Ptr rb_block_proc;
    internal static RB_PtrPtr rb_yield;
    internal static RB_PtrPtrPtr rb_ivar_get;
    internal static RB_PtrPtrPtrPtr rb_ivar_set;
    internal static RB_PtrPtrPtrInt rb_range_new;
    internal static RB_IntPtrRefPtrRefPtrRefInt rb_range_values;
    internal static RB_PtrPtrInt rb_reg_new_str;
    internal static RB_PtrStrStr rb_file_open;
    internal static RB_PtrPtrPtr rb_time_num_new;
    #endregion

    private enum ErrorType
    {
        RuntimeError,
        ArgumentError,
        TypeError,
        SystemExit
    }
}

