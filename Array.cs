using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace rubydotnet
{
    public static partial class Ruby
    {
        public class Array : Object
        {
            public new static string KlassName = "Array";
            public new static Class Class { get { return (Class) GetKlass(KlassName); } }

            public Array(IntPtr Pointer) : base(Pointer, true) { }

            public Array(params Object[] objects) : this()
            {
                for (int i = 0; i < objects.Length; i++)
                {
                    this[i] = objects[i];
                }
            }

            public Array() : base(rb_ary_new(), false) { }

            public override string ToString()
            {
                string String = "[";
                for (int i = 0; i < Length; i++)
                {
                    String += this[i].Inspect();
                    if (i != Length - 1) String += ", ";
                }
                return String + "]";
            }

            public int Length
            {
                get => Funcall("length").Convert<Integer>();
            }

            public Object this[Integer Index]
            {
                get => Funcall("[]", Index);
                set => Funcall("[]=", Index, value);
            }
            public Object this[Integer Index, Integer Length]
            {
                get => Funcall("[]", Index, Length);
                set => Funcall("[]=", Index, Length, value);
            }
            public Object this[Range range]
            {
                get => Funcall("[]", range);
                set => Funcall("[]=", range, value);
            }

            public void Expect(params int[] Lengths)
            {
                if (!Lengths.Contains(Length))
                {
                    string expected;
                    if (Lengths.Length == 1) expected = Lengths[0].ToString();
                    else
                    {
                        expected = "one of ";
                        for (int i = 0; i < Lengths.Length; i++)
                        {
                            expected += Lengths[i];
                            if (i != Lengths.Length - 1) expected += ", ";
                        }
                    }
                    Raise(ErrorType.ArgumentError, $"wrong number of arguments (given {Length}, expected {expected})");
                }
            }
            public void Expect(Range r)
            {
                if (Length < r.Min || Length >= r.Max) Raise(ErrorType.ArgumentError, $"wrong number of arguments (given {Length}, expected {r.ToString()})");
            }

            public bool Include(Object o)
            {
                return Funcall("include?", o) == True;
            }
        }
    }
}
