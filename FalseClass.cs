using System;
using System.Collections.Generic;
using System.Text;

namespace rubydotnet
{
    public static partial class Ruby
    {
        public class FalseClass : Object
        {
            public new static string KlassName = "FalseClass";
            public new static Class Class { get { return (Class) GetKlass(KlassName); } }

            public FalseClass() : base(QFalse, false) { }

            public override string ToString()
            {
                return "false";
            }

            public static implicit operator bool(FalseClass f) => false;
        }
    }
}
