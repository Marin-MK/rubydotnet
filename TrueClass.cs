using System;
using System.Collections.Generic;
using System.Text;

namespace rubydotnet
{
    public static partial class Ruby
    {
        public class TrueClass : Object
        {
            public new static string KlassName = "TrueClass";
            public new static Class Class { get { return (Class) GetKlass(KlassName); } }

            public TrueClass() : base(QTrue, false) { }

            public override string ToString()
            {
                return "true";
            }

            public static implicit operator bool(TrueClass t) => true;
        }
    }
}
