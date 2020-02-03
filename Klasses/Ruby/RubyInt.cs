using System;
using System.Collections.Generic;
using System.Text;

namespace RubyDotNET
{
    public class RubyInt : RubyObject
    {
        public RubyInt(IntPtr Pointer)
            : base(Pointer)
        {

        }

        public RubyInt(long Value)
        {
            this.Pointer = Internal.LONG2NUM(Value);
        }

        public int ToInt32()
        {
            AssertUndisposed();
            return (int) Internal.NUM2LONG(this.Pointer);
        }

        public long ToInt64()
        {
            AssertUndisposed();
            return Internal.NUM2LONG(this.Pointer);
        }

        public override string ToString()
        {
            AssertUndisposed();
            return ToInt32().ToString();
        }
    }
}
