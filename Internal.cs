using System;
using System.Collections.Generic;
using System.IO;
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

        /// <summary>
        /// Evaluates a piece of Ruby code.
        /// </summary>
        /// <param name="Code">The string of code to evaluate.</param>
        /// <param name="PrintError">Whether a given error should be printed to the console.</param>
        /// <param name="RaiseError">Whether a given error should be re-raised as a C# error.</param>
        /// <returns></returns>
        public static IntPtr Eval(string Code, bool PrintError = false, bool ThrowError = true)
        {
            IntPtr State = IntPtr.Zero;
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
            //RubyMethod meth = delegate (int Argc, IntPtr[] Argv, IntPtr Self)
            //{
            //    return rb_eval_string(Code);
            //};
            //IntPtr Result = rb_protect(meth, IntPtr.Zero, State);
            //Console.WriteLine(State);
            return Result;
        }

        public static void PrintObject(RubyObject obj)
        {
            SetGlobalVariable("$_temp", obj.Pointer);
            Eval("p $_temp");
            SetGlobalVariable("$_temp", QNil);
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

        public const string RubyPath = "lib/ruby.dll";

        #region Constants
        public static int LONG_MAX = 2147483647;
        public static int LONG_MIN = -LONG_MAX - 1;

        public static int RUBY_FIXNUM_FLAG = 0x01;
        public static int RUBY_FIXNUM_MAX = LONG_MAX >> 1;
        public static int RUBY_FIXNUM_MIN = LONG_MIN >> 1;
        #endregion

        #region Other
        [DllImport(RubyPath)]
        public static extern void ruby_init();

        [DllImport(RubyPath)]
        public static extern IntPtr rb_eval_string(string Code);

        [DllImport(RubyPath)]
        public static extern IntPtr rb_eval_string_protect(string Code, ref IntPtr State);

        [DllImport(RubyPath)]
        public static extern IntPtr rb_protect(RubyMethod Function, IntPtr Arg, IntPtr State);

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
}
