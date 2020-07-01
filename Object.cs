using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;

namespace rubydotnet
{
    public static partial class Ruby
    {
        public class Object
        {
            public static string KlassName = "Object";
            public static Class Class;
            public IntPtr Pointer;
            bool Pinned = false;

            public Object(IntPtr Pointer, bool Validate)
            {
                this.Pointer = Pointer;
                if (Validate && GetType() != typeof(Object))
                {
                    IntPtr cls = rb_funcallv(Pointer, rb_intern("class"), 0);
                    IntPtr strptr = rb_funcallv(cls, rb_intern("to_s"), 0);
                    string str = NUM2STR(strptr);
                    Type t = GetType();
                    string klassname = null;
                    if (GetType() == typeof(Class)) klassname = "Class";
                    else if (GetType() == typeof(Module)) klassname = "Module";
                    else
                    {
                        FieldInfo info = t.GetField("KlassName", BindingFlags.Public | BindingFlags.Static);
                        klassname = (string) info.GetValue(null);
                    }
                    if (str != klassname) throw new Exception($"Attempted to create {klassname} object from {str} object.");
                }
            }

            public Object(Object Object, bool Validate = true) : this(Object.Pointer, Validate) { }

            ~Object()
            {
                if (Pinned) Console.WriteLine("A pinned C# object has been disposed. It must be unpinned, otherwise the associated Ruby object will live on forever, creating a memory leak.");
            }

            public static Object GetKlass(string Name)
            {
                if (Name == "Object") return Object.Class;
                foreach (KeyValuePair<Class, Type> kvp in Class.CustomClasses)
                {
                    if (kvp.Key.Name == Name) return kvp.Key;
                }
                foreach (KeyValuePair<Module, Type> kvp in Module.CustomModules)
                {
                    if (kvp.Key.Name == Name) return kvp.Key;
                }
                Object obj = Object.Class.Funcall("const_get", (String) Name);
                if (obj.Pointer == QNil)
                {
                    throw new Exception($"Attempted to get klass by name of {Name}, but it did not exist.");
                }
                else
                {
                    String cstr = obj.Funcall("class").Funcall("to_s").Convert<String>();
                    if (cstr == "Class") return obj.Convert<Class>(Name);
                    else if (cstr == "Module") return obj.Convert<Module>(Name);
                    else
                    {
                        throw new Exception($"Attempted to get klass by name of {Name}, but it returned {cstr}.");
                    }
                }
            }

            public T Convert<T>(string KlassName = null) where T : Object
            {
                if (GetType() == typeof(T)) return (T) this;
                if (typeof(T) == typeof(NilClass) || typeof(T) == typeof(TrueClass) || typeof(T) == typeof(FalseClass))
                    return (T) Activator.CreateInstance(typeof(T));
                else if (typeof(T) == typeof(Class) || typeof(T) == typeof(Module))
                {
                    return (T) Activator.CreateInstance(typeof(T), this.Pointer, KlassName);
                }
                return (T) Activator.CreateInstance(typeof(T), this.Pointer);
            }

            public bool Is(Class Class)
            {
                return Funcall("class").Funcall("to_s").Convert<String>() == Class.Name;
            }
            public bool Is(Module Module)
            {
                return Funcall("class").Funcall("to_s").Convert<String>() == Module.Name;
            }
            public bool IsClass()
            {
                return Funcall("is_a?", Eval("Class")).Pointer == QTrue;
            }
            public bool IsModule()
            {
                return Funcall("is_a?", Eval("Module")).Pointer == QTrue;
            }

            public bool IsNil()
            {
                return this.Pointer == QNil;
            }

            public Object Pin()
            {
                if (Pinned) throw new Exception("Object already pinned.");
                Pinned = true;
                if (rb_gv_get("$__rdncache__") == QNil)
                    rb_gv_set("$__rdncache__", rb_ary_new());
                if (rb_funcallv(rb_gv_get("$__rdncache__"), rb_intern("include?"), 1, new IntPtr[] { this.Pointer }) == QFalse)
                    rb_funcallv(rb_gv_get("$__rdncache__"), rb_intern("push"), 1, new IntPtr[] { this.Pointer });
                return this;
            }

            public void Unpin()
            {
                if (!Pinned) throw new Exception("Object not pinned.");
                Pinned = false;
                if (rb_gv_get("$__rdncache__") == QNil) return;
                if (rb_funcallv(rb_gv_get("$__rdncache__"), rb_intern("include?"), 1, new IntPtr[] { this.Pointer }) == QTrue)
                    rb_funcallv(rb_gv_get("$__rdncache__"), rb_intern("delete"), 1, new IntPtr[] { this.Pointer });
            }

            public void Expect(params Class[] Classes)
            {
                for (int i = 0; i < Classes.Length; i++)
                {
                    if (Is(Classes[i])) return;
                }
                string expected = "";
                if (Classes.Length == 1) expected = Classes[0].Name;
                else
                {
                    expected += "one of ";
                    for (int i = 0; i < Classes.Length; i++)
                    {
                        expected += Classes[i].Name;
                        if (i != Classes.Length - 1) expected += ", ";
                    }
                }
                Raise(ErrorType.TypeError, $"expected {expected}, but got {Funcall("class").Funcall("to_s").Convert<String>()} instead.");
            }

            public override string ToString()
            {
                return Funcall("to_s").Convert<String>();
            }

            public virtual string Inspect()
            {
                return ToString();
            }

            public Object Funcall(IntPtr Function, params Object[] Values)
            {
                return new Object(SafeRuby(delegate (IntPtr Arg) { return rb_funcallv(this.Pointer, Function, Values.Length, Values.Select(o => o.Pointer).ToArray()); }), false);
            }
            public Object Funcall(IntPtr Function, Array Values)
            {
                IntPtr[] Pointers = new IntPtr[Values.Length];
                for (int i = 0; i < Values.Length; i++) Pointers[i] = Values[i].Pointer;
                IntPtr Result = SafeRuby(delegate (IntPtr Arg) { return rb_funcallv(this.Pointer, Function, Values.Length, Pointers); });
                return new Object(Result, false);
            }
            public Object Funcall(string Function, params Object[] Values)
            {
                return Funcall(rb_intern(Function), Values);
            }
            public Object Funcall(string Function, Array Values)
            {
                return Funcall(rb_intern(Function), Values);
            }

            public Object Funcall(IntPtr Function, Proc Proc, params Object[] Values)
            {
                return new Object(SafeRuby(delegate (IntPtr Arg)
                {
                    return rb_block_call(this.Pointer, Function, Values.Length, Values.Select(o => o.Pointer).ToArray(), delegate (IntPtr Arg, IntPtr Data, int Argc, IntPtr[] Argv)
                    {
                        return Proc(new Object(Arg, false)).Pointer;
                    }, QNil);
                }), false);
            }
            public Object Funcall(IntPtr Function, Proc Proc, Array Values)
            {
                IntPtr[] Pointers = new IntPtr[Values.Length];
                for (int i = 0; i < Values.Length; i++) Pointers[i] = Values[i].Pointer;
                return new Object(SafeRuby(delegate (IntPtr Arg)
                {
                    return rb_block_call(this.Pointer, Function, Values.Length, Pointers, delegate (IntPtr Arg, IntPtr Data, int Argc, IntPtr[] Argv)
                    {
                        return Proc(new Object(Arg, false)).Pointer;
                    }, QNil);
                }), false);
            }
            public Object Funcall(string Function, Proc Proc, params Object[] Values)
            {
                return Funcall(rb_intern(Function), Proc, Values);
            }
            public Object Funcall(string Function, Proc Proc, Array Values)
            {
                return Funcall(rb_intern(Function), Proc, Values);
            }

            public Object GetIVar(string Name)
            {
                return new Object(rb_ivar_get(this.Pointer, rb_intern(Name)), false);
            }

            public Object SetIVar(string Name, Object Value)
            {
                return new Object(rb_ivar_set(this.Pointer, rb_intern(Name), Value.Pointer), false);
            }
        }
    }
}
