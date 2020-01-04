using System;
using System.Collections.Generic;
using System.Text;

namespace RubyDotNET
{
    public class RubyObject : Klass
    {
        public static Class CreateClass(IntPtr Pointer)
        {
            if (!Internal.IsType(Pointer, Internal.T_CLASS))
                throw new Exception("Can't create a class from a non-class pointer");
            Class c = new Class("Object", Pointer);
            return c;
        }

        public RubyObject()
        {

        }

        public RubyObject(IntPtr Pointer, int Type = 0x01)
        {
            this.Pointer = Pointer;
            if (!Internal.IsType(Pointer, Type))
                throw new Exception("Invalid data type.");
        }

        public override string ToString()
        {
            return this.GetType().ToString();
        }

        public T Convert<T>()
        {
            return (T) Activator.CreateInstance(typeof(T), this.Pointer);
        }

        public void Print()
        {
            Internal.PrintObject(this);
        }

        public bool IsNil()
        {
            return Pointer == Internal.QNil;
        }
    }
}
