using System;
using System.Collections.Generic;
using System.Text;

namespace RubyDotNET
{
    public class RubyHash : RubyObject
    {
        public new static Class CreateClass(IntPtr Pointer)
        {
            Class c = new Class("Hash", Pointer);
            return c;
        }

        public RubyHash(IntPtr Pointer)
            : base(Pointer)
        {

        }

        public RubyHash()
        {
            this.Pointer = Internal.rb_hash_new();
        }

        public RubyObject this[RubyObject Key]
        {
            get
            {
                return new RubyObject(Internal.rb_hash_aref(this.Pointer, Key.Pointer));
            }
            set
            {
                Internal.rb_hash_aset(this.Pointer, Key.Pointer, value.Pointer);
            }
        }

        public int Length
        {
            get
            {
                return (int) Internal.NUM2LONG(Internal.rb_hash_size(this.Pointer));
            }
        }

        public RubyArray Keys
        {
            get
            {
                return new RubyArray(Internal.rb_funcallv(this.Pointer, Internal.rb_intern("keys"), 0));
            }
        }

        public override string ToString()
        {
            return "{..." + Length.ToString() + "...}";
        }
    }
}
