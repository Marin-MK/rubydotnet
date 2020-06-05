using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace RubyDotNET
{
    public class RubyString : RubyObject
    {
        public RubyString(IntPtr Data)
            : base(Data)
        {
        
        }

        public RubyString(string String)
        {
            this.Pointer = Internal.rb_str_new_cstr(String);
        }

        public override string ToString()
        {
            AssertUndisposed();
            long len = Internal.rb_str_strlen(this.Pointer);
            return Marshal.PtrToStringUTF8(Internal.rb_string_value_ptr(ref this.Pointer));
        }
    }
}
