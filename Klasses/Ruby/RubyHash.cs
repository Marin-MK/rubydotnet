using System;
using System.Collections.Generic;
using System.Text;

namespace RubyDotNET
{
    public class RubyHash : RubyObject
    {
        public RubyHash(IntPtr Pointer)
            : base(Pointer, Internal.T_HASH)
        {

        }

        public RubyHash()
        {
            
        }

        public RubyObject this[RubyObject Key]
        {
            get
            {
                AssertUndisposed();
                return new RubyObject(Internal.rb_hash_aref(this.Pointer, Key.Pointer));
            }
            set
            {
                AssertUndisposed();
                Internal.rb_hash_aset(this.Pointer, Key.Pointer, value.Pointer);
            }
        }

        public int Length
        {
            get
            {
                AssertUndisposed();
                return (int) Internal.NUM2LONG(Internal.rb_hash_size(this.Pointer));
            }
        }

        public RubyArray Keys
        {
            get
            {
                AssertUndisposed();
                return new RubyArray(Internal.rb_funcallv(this.Pointer, Internal.rb_intern("keys"), 0));
            }
        }

        public RubyObject GetKey(Predicate<RubyObject> match)
        {
            AssertUndisposed();
            RubyArray keys = this.Keys;
            for (int i = 0; i < keys.Length; i++)
            {
                if (match(keys[i])) return keys[i];
            }
            return null;
        }

        public override string ToString()
        {
            AssertUndisposed();
            return "{... (size " + Length.ToString() + ")}";
        }
    }
}
