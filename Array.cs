using System;

namespace rubydotnet;

public static partial class Ruby
{
    public static class Array
    {
        public static long Length(IntPtr Object)
        {
            return Integer.FromPtr(Funcall(Object, "length"));
        }

        public static IntPtr Get(IntPtr Object, int Index)
        {
            return rb_ary_entry(Object, Index);
        }

        public static void Set(IntPtr Object, int Index, IntPtr Value)
        {
            rb_ary_store(Object, Index, Value);
        }

        public static IntPtr Create(int Size = 0)
        {
            IntPtr Value = rb_ary_new();
            for (int i = 0; i < Size; i++) rb_ary_store(Value, i, Nil);
            return Value;
        }

        public static IntPtr Create(int Size, IntPtr Element)
        {
            IntPtr Value = rb_ary_new();
            for (int i = 0; i < Size; i++) rb_ary_store(Value, i, Element);
            return Value;
        }

        public static bool Is(IntPtr Object, int Index, params string[] Class)
        {
            return Ruby.Is(Get(Object, Index), Class);
        }

        public static void Expect(IntPtr Object, int Index, params string[] Class)
        {
            Ruby.Expect(Get(Object, Index), Class);
        }

        public static void Expect(IntPtr Object, int Expected)
        {
            long len = Length(Object);
            if (len != Expected) Raise(ErrorType.ArgumentError, $"wrong number of arguments (given {len}, expected {Expected})");
        }

        public static void Expect(IntPtr Object, params int[] Expected)
        {
            long len = Length(Object);
            for (int i = 0; i < Expected.Length; i++)
            {
                if (len == Expected[i]) return;
            }
            string expectedstr = "";
            for (int i = 0; i < Expected.Length; i++)
            {
                expectedstr += Expected[i].ToString();
                if (i == Expected.Length - 2) expectedstr += " or ";
                else if (i < Expected.Length - 2) expectedstr += ", ";
            }
            Raise(ErrorType.ArgumentError, $"wrong number of arguments (given {Length(Object)}, expected {expectedstr})");
        }

        internal static RB_Ptr rb_ary_new;
        internal static RB_PtrPtrInt rb_ary_entry;
        internal static RB_VoidPtrLngPtr rb_ary_store;
    }
}
