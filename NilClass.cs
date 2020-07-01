using System;
using System.Collections.Generic;
using System.Text;

namespace rubydotnet
{
    public static partial class Ruby
    {
        public class NilClass : Object
        {
            public new static string KlassName = "NilClass";
            public new static Class Class { get { return (Class) GetKlass(KlassName); } }

            public NilClass() : base(QNil, false) { }

            public override string ToString()
            {
                return "nil";
            }
        }
    }
}
