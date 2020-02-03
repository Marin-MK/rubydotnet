using System;
using System.Collections.Generic;
using System.Text;

namespace RubyDotNET
{
    public class Table : RubyObject
    {
        public static IntPtr ClassPointer;

        public static Class CreateClass()
        {
            Class c = new Class("Table");
            ClassPointer = c.Pointer;
            c.DefineClassMethod("_load", _load, -2);
            c.DefineMethod("size", sizeget, -2);
            c.DefineMethod("size=", sizeset, -2);
            return c;
        }

        public Table(IntPtr Pointer)
            : base(Pointer)
        {

        }

        public Table()
        {
            this.Pointer = Internal.Eval("Table.new");
        }

        public static IntPtr sizeget(IntPtr _self, IntPtr _args)//int argc, IntPtr[] argv, IntPtr self)
        {
            return Internal.rb_ivar_get(_self, Internal.rb_intern("@size"));
        }

        public static IntPtr sizeset(IntPtr _self, IntPtr _args)//int argc, IntPtr[] argv, IntPtr self)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            //IntPtr[] Values = new IntPtr[1];
            //Internal.rb_scan_args(argc, argv, "1", Values);
            return Internal.rb_ivar_set(_self, Internal.rb_intern("@size"), Args[0].Pointer);
        }

        public static IntPtr _load(IntPtr _self, IntPtr _args)//int argc, IntPtr[] argv, IntPtr self)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);

            //IntPtr[] Values = new IntPtr[1];

            //Internal.rb_scan_args(argc, argv, "1", Values);

            RubyArray ary = new RubyArray(Internal.rb_funcallv(Args[0].Pointer, Internal.rb_intern("unpack"), 1, Internal.rb_str_new_cstr("LLLLLS*")));

            IntPtr obj = Internal.rb_funcallv(Table.ClassPointer, Internal.rb_intern("allocate"), 0);

            Internal.rb_ivar_set(obj, Internal.rb_intern("@size"), ary[0].Pointer);
            Internal.rb_ivar_set(obj, Internal.rb_intern("@xsize"), ary[1].Pointer);
            Internal.rb_ivar_set(obj, Internal.rb_intern("@ysize"), ary[2].Pointer);
            Internal.rb_ivar_set(obj, Internal.rb_intern("@zsize"), ary[3].Pointer);
            // Fifth argument unused; data size (equal to @size)
            // Set @data to elements 5 to -1 and drop @size, @xsize, @ysize, @zsize and data_size.
            Internal.rb_ivar_set(obj, Internal.rb_intern("@data"), Internal.rb_funcallv(ary.Pointer, Internal.rb_intern("drop"), 1, Internal.LONG2NUM(5)));

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
