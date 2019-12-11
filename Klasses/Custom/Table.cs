using System;
using System.Collections.Generic;
using System.Text;

namespace RubyDotNET
{
    public class Table : RubyObject
    {
        public static Class CreateClass()
        {
            Class c = new Class("Table");
            c.DefineClassMethod("_load", _load);
            c.DefineMethod("size", size);
            c.DefineMethod("size=", size_set);
            return c;
        }

        public Table(IntPtr Pointer)
            : base(Pointer)
        {

        }

        public Table()
        {

        }

        public static IntPtr size(int Argc, IntPtr[] Argv, IntPtr Self)
        {
            return Internal.rb_ivar_get(Self, Internal.rb_intern("@size"));
        }

        public static IntPtr size_set(int Argc, IntPtr[] Argv, IntPtr Self)
        {
            IntPtr[] Values = new IntPtr[1];

            Internal.rb_scan_args(Argc, Argv, "1", Values);

            IntPtr sizeptr = Values[0];

            Internal.rb_ivar_set(Self, Internal.rb_intern("@size"), sizeptr);

            return Self;
        }

        public static IntPtr _load(int Argc, IntPtr[] Argv, IntPtr Self)
        {
            IntPtr[] Values = new IntPtr[1];

            Internal.rb_scan_args(Argc, Argv, "1", Values);

            IntPtr str = Values[0];

            RubyArray ary = new RubyArray(Internal.rb_funcallv(str, Internal.rb_intern("unpack"), 1, new IntPtr[1] { Internal.rb_str_new_cstr("LLLLLS*") }));

            IntPtr obj = Internal.rb_funcallv(Internal.GetKlass("Table").Pointer, Internal.rb_intern("new"), 0);

            Internal.rb_ivar_set(obj, Internal.rb_intern("@size"), ary[0].Pointer);
            Internal.rb_ivar_set(obj, Internal.rb_intern("@xsize"), ary[1].Pointer);
            Internal.rb_ivar_set(obj, Internal.rb_intern("@ysize"), ary[2].Pointer);
            Internal.rb_ivar_set(obj, Internal.rb_intern("@zsize"), ary[3].Pointer);
            Internal.rb_ivar_set(obj, Internal.rb_intern("@data"), Internal.rb_funcallv(ary.Pointer, Internal.rb_intern("drop"), 1, new IntPtr[1] { Internal.LONG2NUM(4) }));

            return obj;
        }

        public RubyInt Size
        {
            get
            {
                return new RubyInt(GetIVar("@size"));
            }
        }

        public RubyInt XSize
        {
            get
            {
                return new RubyInt(GetIVar("@xsize"));
            }
        }

        public RubyInt YSize
        {
            get
            {
                return new RubyInt(GetIVar("@ysize"));
            }
        }

        public RubyInt ZSize
        {
            get
            {
                return new RubyInt(GetIVar("@zsize"));
            }
        }

        public RubyArray Data
        {
            get
            {
                return new RubyArray(GetIVar("@data"));
            }
        }
    }
}
