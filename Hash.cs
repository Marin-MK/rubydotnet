using System;
using System.Collections.Generic;
using System.Text;

namespace rubydotnet
{
    public static partial class Ruby
    {
        public class Hash : Object
        {
            public new static string KlassName = "Hash";
            public new static Class Class { get { return (Class) GetKlass(KlassName); } }

            public Hash(IntPtr Pointer) : base(Pointer, true) { }

            public Hash() : base(rb_hash_new(), false) { }

            public Array Keys
            {
                get => new Array(rb_hash_keys(this.Pointer));
            }

            public Array Values
            {
                get => new Array(rb_hash_values(this.Pointer));
            }

            public int Length
            {
                get => new Integer(rb_hash_size(this.Pointer));
            }

            public Object this[Object Key]
            {
                get => new Object(rb_hash_aref(this.Pointer, Key.Pointer), false);
                set => rb_hash_aset(this.Pointer, Key.Pointer, value.Pointer);
            }

            public override string ToString()
            {
                string String = "{";
                Array keys = this.Keys;
                for (int i = 0; i < keys.Length; i++)
                {
                    String += keys[i].Inspect() + " => " + this[keys[i]].Inspect();
                    if (i != Length - 1) String += ", ";
                }
                return String + "}";
            }
        }
    }
}
