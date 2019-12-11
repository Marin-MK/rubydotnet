using System;
using System.Collections.Generic;
using System.Text;

namespace RubyDotNET
{
    public class RubyFile : RubyObject
    {
        public new static Class CreateClass(IntPtr Pointer)
        {
            Class c = new Class("File", Pointer);
            return c;
        }

        public static RubyFile Open(string Filename, string Mode)
        {
            IntPtr ptr = Internal.rb_file_open(Filename, Mode);
            return new RubyFile(ptr);
        }

        public RubyFile(IntPtr Value)
            : base(Value) { }

        public RubyString Read()
        {
            return new RubyString(Internal.rb_funcallv(this.Pointer, Internal.rb_intern("read"), 0));
        }

        public void Close()
        {
            Internal.rb_io_close(this.Pointer);
        }
    }
}
