using System;
using System.Collections.Generic;
using System.Text;

namespace rubydotnet
{
    public static partial class Ruby
    {
        public class Range : Object
        {
            public new static string KlassName = "Range";
            public new static Class Class { get { return (Class) GetKlass(KlassName); } }

            public Range(IntPtr Pointer) : base(Pointer, true) { }

            public Range(int Min, int Max, bool Inclusive = false) : base(rb_range_new(INT2NUM(Min), INT2NUM(Max), Inclusive ? 0 : 1), false) { }

            public (int Min, int Max, bool Inclusive) Values
            {
                get
                {
                    IntPtr Min = IntPtr.Zero;
                    IntPtr Max = IntPtr.Zero;
                    int Type = -1;
                    rb_range_values(this.Pointer, ref Min, ref Max, ref Type);
                    return (NUM2INT(Min), NUM2INT(Max), Type == 0);
                }
            }

            public int Min
            {
                get => Values.Min;
            }

            public int Max
            {
                get => Values.Max + (Values.Inclusive ? 1 : 0);
            }

            public bool Inclusive
            {
                get => Values.Inclusive;
            }

            public override string ToString()
            {
                if (Inclusive) return $"{Min}..{Max - 1}";
                else return $"{Min}...{Max}";
            }

            public static implicit operator Range((int Min, int Max, bool Inclusive) r) => new Range(r.Min, r.Max, r.Inclusive);
            public static implicit operator Range((int Min, int Max) r) => new Range(r.Min, r.Max, false);
        }
    }
}
