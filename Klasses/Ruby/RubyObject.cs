using System;
using System.Collections.Generic;
using System.Text;

namespace RubyDotNET
{
    [System.Diagnostics.DebuggerDisplay("ptr = {Pointer}")]
    public class RubyObject : Klass
    {
        public static Class CreateClass(IntPtr Pointer)
        {
            Class c = new Class("Object", Pointer);
            return c;
        }

        public RubyObject()
        {

        }

        public RubyObject(IntPtr Pointer)
        {
            this.Pointer = Pointer;
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
    }
}
