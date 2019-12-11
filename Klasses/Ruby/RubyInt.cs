using System;
using System.Collections.Generic;
using System.Text;

namespace RubyDotNET
{
    public class RubyInt : RubyObject
    {
        public new static Class CreateClass(IntPtr Pointer)
        {
            Class c = new Class("Fixnum", Pointer);
            return c;
        }

        public RubyInt(IntPtr Pointer)
            : base(Pointer)
        {

        }

        public RubyInt()
        {

        }

        public RubyInt(long Value)
        {
            this.Pointer = Internal.LONG2NUM(Value);
        }

        public int ToInt32()
        {
            return (int) Internal.NUM2LONG(this.Pointer);
        }

        public long ToInt64()
        {
            return Internal.NUM2LONG(this.Pointer);
        }

        public override string ToString()
        {
            return ToInt32().ToString();
        }
    }
}
