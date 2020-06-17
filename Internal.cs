using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace RubyDotNET
{
    public static class Internal
    {
        public const string RubyPath = "lib/x64-msvcrt-ruby270.dll";

        public static Dictionary<IntPtr, Klass> Klasses = new Dictionary<IntPtr, Klass>();
        public static Class rb_cObject;
        public static Class rb_cString;
        public static Class rb_cArray;
        public static Class rb_cFile;
        public static Class rb_cDir;
        public static Class rb_cThread;
        public static Class rb_eArgumentError;
        public static Class rb_eRuntimeError;
        public static Class rb_eSystemExit;
        public static Class ObjectSpace;
        public static bool Initialized = false;

        public static Random Random;

        public static void Initialize()
        {
            if (Initialized) return;
            int val = 0;
            IntPtr valptr = Marshal.AllocHGlobal(sizeof(int));
            Marshal.StructureToPtr(val, valptr, false);
            string[] data = new string[1] { "" };
            IntPtr ptr = Marshal.UnsafeAddrOfPinnedArrayElement(data, 0);
            ruby_sysinit(valptr, ptr);
            ruby_init();
            rb_cObject = RubyObject.CreateClass(Eval("Object"));
            rb_cString = RubyObject.CreateClass(Eval("String"));
            rb_cArray = RubyObject.CreateClass(Eval("Array"));
            rb_cFile = RubyObject.CreateClass(Eval("File"));
            rb_cDir = RubyObject.CreateClass(Eval("Dir"));
            rb_cThread = RubyObject.CreateClass(Eval("Thread"));
            rb_eArgumentError = RubyObject.CreateClass(Eval("ArgumentError"));
            rb_eRuntimeError = RubyObject.CreateClass(Eval("RuntimeError"));
            rb_eSystemExit = RubyObject.CreateClass(Eval("SystemExit"));
            QNil = Eval("nil");
            QTrue = Eval("true");
            QFalse = Eval("false");
            ObjectSpace = RubyObject.CreateClass(Eval("ObjectSpace"));
            Initialized = true;
            Random = new Random();
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

        public static IntPtr SetIVar(IntPtr Self, string Name, IntPtr Value)
        {
            return rb_ivar_set(Self, rb_intern(Name), Value);
        }

        public static IntPtr GetIVar(IntPtr Self, string Name)
        {
            return rb_ivar_get(Self, rb_intern(Name));
        }

        /// <summary>
        /// Evaluates a piece of Ruby code.
        /// </summary>
        /// <param name="Code">The string of code to evaluate.</param>
        /// <param name="PrintError">Whether a given error should be printed to the console.</param>
        /// <param name="RaiseError">Whether a given error should be re-raised as a C# error.</param>
        /// <returns></returns>
        public static IntPtr Eval(string Code, bool PrintError = true, bool ThrowError = true)
        {
            IntPtr State = (IntPtr) 0;
            IntPtr Result = rb_eval_string_protect(Code, ref State);
            if (State != IntPtr.Zero)
            {
                if (!ThrowError && !PrintError) return IntPtr.Zero;
                string type = new RubyString(rb_eval_string("$!.class.to_s")).ToString();
                string msg = type + ": " + new RubyString(rb_eval_string("$!.to_s")).ToString();
                RubyArray stack = new RubyArray(rb_eval_string("$!.backtrace"));
                if (stack.Length > 0) msg += "\n\n";
                for (int i = 0; i < stack.Length; i++)
                {
                    string line = stack[i].Convert<RubyString>().ToString();
                    string[] colons = line.Split(':');
                    int linenum = Convert.ToInt32(colons[1]);
                    msg += "Line " + linenum.ToString() + ": ";
                    for (int j = 2; j < colons.Length; j++)
                    {
                        if (colons[j].Length > 3 && colons[j][0] == 'i' && colons[j][1] == 'n' && colons[j][2] == ' ')
                            colons[j] = colons[j].Replace("in ", "");
                        msg += colons[j];
                        if (j != colons.Length - 1) msg += ":";
                    }
                    if (i != stack.Length - 1) msg += "\n";
                }
                msg = msg[0].ToString().ToUpper() + msg.Substring(1);
                if (PrintError) Console.WriteLine(msg);
                if (ThrowError) throw new Exception(msg);
            }
            return Result;
        }

        public static string FormatError(IntPtr Error)
        {
            Console.WriteLine("started");
            string type = new RubyString(rb_funcallv(rb_funcallv(Error, rb_intern("class"), 0), rb_intern("to_s"), 0)).ToString();
            string msg = type + ": " + new RubyString(rb_funcallv(Error, rb_intern("to_s"), 0)).ToString();
            RubyArray stack = new RubyArray(rb_funcallv(Error, rb_intern("backtrace"), 0));
            if (stack.Length > 0) msg += "\n\n";
            for (int i = 0; i < stack.Length; i++)
            {
                string line = stack[i].Convert<RubyString>().ToString();
                string[] colons = line.Split(':');
                int linenum = Convert.ToInt32(colons[1]);
                msg += "Line " + linenum.ToString() + ": ";
                for (int j = 2; j < colons.Length; j++)
                {
                    if (colons[j].Length > 3 && colons[j][0] == 'i' && colons[j][1] == 'n' && colons[j][2] == ' ')
                        colons[j] = colons[j].Replace("in ", "");
                    msg += colons[j];
                    if (j != colons.Length - 1) msg += ":";
                }
                if (i != stack.Length - 1) msg += "\n";
            }
            msg = msg[0].ToString().ToUpper() + msg.Substring(1);
            return msg;
        }

        public static void PrintObject(RubyObject obj)
        {
            obj.AssertUndisposed();
            SetGlobalVariable("$_temp", obj.Pointer);
            Eval("p $_temp");
            SetGlobalVariable("$_temp", QNil);
        }

        public static int rb_type(IntPtr Obj)
        {
            if (RB_IMMEDIATE_P(Obj))
            {
                if (RB_FIXNUM_P(Obj)) return T_FIXNUM;
                if (RB_FLONUM_P(Obj)) return T_FLOAT;
                if (Obj == QTrue) return T_TRUE;
                if (RB_STATIC_SYM_P(Obj)) return T_SYMBOL;
                if (Obj == QUndef) return T_UNDEF;
            }
            else if (!RB_TEST(Obj))
            {
                if (Obj == QNil) return T_NIL;
                if (Obj == QFalse) return T_FALSE;
            }
            return RB_BUILTIN_TYPE(Obj);
        }

        #region Helper functions
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
            return (IntPtr)((v << 1) | RUBY_FIXNUM_FLAG);
#pragma warning restore CS0675 // Bitwise-or operator used on a sign-extended operand
        }

        public static long NUM2LONG(IntPtr Value)
        {
            if (RB_FIXNUM_P(Value)) return (int) Value >> 1;
            return rb_num2ll(Value);
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

        public static bool RB_IMMEDIATE_P(IntPtr v)
        {
            return ((long)(v) & IMMEDIATE_MASK) == 1;
        }

        public static bool RB_FIXNUM_P(IntPtr v)
        {
            return (((int)(long)(v)) & FIXNUM_FLAG) == 1;
        }

        public static bool RB_FLONUM_P(IntPtr x)
        {
            return (((int)(long)(x)) & FLONUM_MASK) == FLONUM_FLAG;
        }

        public static bool RB_SYMBOL_P(IntPtr x)
        {
            return (((long)(x) & ~((~(long)0) << RUBY_SPECIAL_SHIFT)) == SYMBOL_FLAG);
        }

        private static int RB_BUILTIN_TYPE(IntPtr v)
        {
            long flag = Marshal.ReadInt64(v);
            return (int)(flag & RUBY_T_MASK);
        }

        public static bool RB_STATIC_SYM_P(IntPtr v)
        {
            return (((long)(v) & ~((~(long) 0) << RUBY_SPECIAL_SHIFT)) == RUBY_SYMBOL_FLAG);
        }

        public static bool RB_TEST(IntPtr v)
        {
            return !(((long)(v) & ~(int)QNil) == 0);
        }
        #endregion

        #region Constants
        public static IntPtr QTrue = (IntPtr) 0x14;
        public static IntPtr QFalse = (IntPtr) 0x00;
        public static IntPtr QNil = (IntPtr) 0x08;
        public static IntPtr QUndef = (IntPtr) 0x34;

        public static int LONG_MAX = 2147483647;
        public static int LONG_MIN = -LONG_MAX - 1;

        public static int RUBY_FIXNUM_FLAG = 0x01;
        public static int RUBY_FIXNUM_MAX = LONG_MAX >> 1;
        public static int RUBY_FIXNUM_MIN = LONG_MIN >> 1;

        private const int FIXNUM_FLAG = 0x01;
        private const int FLONUM_MASK = 0x03;
        private const int FLONUM_FLAG = 0x02;
        private const int RUBY_SYMBOL_FLAG = 0x0c;
        private const int IMMEDIATE_MASK = 0x07;
        private const int RUBY_SPECIAL_SHIFT = 8;
        private const int SYMBOL_FLAG = 0x0c;
        private const int RUBY_T_MASK = 0x1f;

        // Class types
        public static int T_OBJECT = 0x01;
        public static int T_CLASS = 0x02;
        public static int T_MODULE = 0x03;
        public static int T_FLOAT = 0x04;
        public static int T_STRING = 0x05;
        public static int T_REGEXP = 0x06;
        public static int T_ARRAY = 0x07;
        public static int T_HASH = 0x08;
        public static int T_STRUCT = 0x09;
        public static int T_BIGNUM = 0x0a;
        public static int T_FILE = 0x0b;
        public static int T_DATA = 0x0c;
        public static int T_MATCH = 0x0d;
        public static int T_COMPLEX = 0x0e;
        public static int T_RATIONAL = 0x0f;

        public static int T_NIL = 0x11;
        public static int T_TRUE = 0x12;
        public static int T_FALSE = 0x13;
        public static int T_SYMBOL = 0x14;
        public static int T_FIXNUM = 0x15;
        public static int T_UNDEF = 0x016;

        #endregion

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr RubyMethod(IntPtr Self, IntPtr Args);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr DangerousFunction(IntPtr Argument);

        #region Imports
        #region Other
        [DllImport(RubyPath)]
        public static extern void ruby_sysinit(IntPtr Argc, IntPtr Argv);

        [DllImport(RubyPath)]
        public static extern void ruby_init();

        [DllImport(RubyPath)]
        public static extern IntPtr rb_eval_string(string Code);

        [DllImport(RubyPath)]
        public static extern IntPtr rb_eval_string_protect(string Code, ref IntPtr State);

        [DllImport(RubyPath)]
        public static extern IntPtr rb_obj_instance_eval(int Argc, IntPtr[] Argv, IntPtr Obj);

        [DllImport(RubyPath)]
        public static extern IntPtr rb_errinfo();

        [DllImport(RubyPath)]
        public static extern void rb_set_errinfo(IntPtr Value);

        [DllImport(RubyPath)]
        public static extern IntPtr rb_protect(DangerousFunction Function, IntPtr Arg, out IntPtr State);

        [DllImport(RubyPath)]
        public static extern void rb_gv_set(string Var, IntPtr Value);

        [DllImport(RubyPath)]
        public static extern IntPtr rb_gv_get(string Var);

        [DllImport(RubyPath)]
        public static extern void rb_global_variable(IntPtr Object);

        [DllImport(RubyPath)]
        public static extern void rb_define_variable(string VarName, IntPtr Object);

        [DllImport(RubyPath)]
        public static extern void rb_raise(IntPtr Class, string Message);

        [DllImport(RubyPath)]
        public static extern void rb_require(string Var);

        [DllImport(RubyPath)]
        public static extern IntPtr rb_const_get(IntPtr Object, IntPtr ID);

        [DllImport(RubyPath)]
        public static extern IntPtr rb_const_set(IntPtr Object, IntPtr ID, IntPtr Value);

        [DllImport(RubyPath)]
        public static extern IntPtr rb_define_const(IntPtr Parent, string Name, IntPtr Value);

        [DllImport(RubyPath)]
        public static extern IntPtr rb_define_global_const(string Name, IntPtr Value);
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

        [DllImport(RubyPath)]
        public static unsafe extern char* rb_string_value_cstr(long* Value);

        [DllImport(RubyPath)]
        public static extern string rb_str_to_cstr(IntPtr Value);
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
        public static extern long rb_num2ll(IntPtr Value);

        [DllImport(RubyPath)]
        public static extern IntPtr rb_int2big(long Value);

        [DllImport(RubyPath)]
        public static extern IntPtr rb_float_new(double Value);

        [DllImport(RubyPath)]
        public static extern double rb_num2dbl(IntPtr Value);
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
        #endregion
    }
}

