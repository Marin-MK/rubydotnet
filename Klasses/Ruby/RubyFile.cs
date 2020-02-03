using System;
using System.Collections.Generic;
using System.Text;

namespace RubyDotNET
{
    public class RubyFile : RubyObject
    {
        public static RubyFile Open(string Filename, string Mode)
        {
            IntPtr ptr = Internal.rb_file_open(Filename, Mode);
            return new RubyFile(ptr);
        }

        public RubyFile(IntPtr Value)
            : base(Value)
        {
            
        }

        public RubyString Read()
        {
            AssertUndisposed();
            return new RubyString(Internal.rb_funcallv(this.Pointer, Internal.rb_intern("read"), 0));
        }

        public void Close()
        {
            AssertUndisposed();
            Internal.rb_io_close(this.Pointer);
        }
    }
}
