using System;
using System.Collections.Generic;
using System.Text;

namespace RubyDotNET
{
    public class RubyObject : Klass
    {
        public bool Freed { get; protected set; } = false;

        public static Class CreateClass(IntPtr Pointer)
        {
            Class c = new Class("Object", Pointer);
            return c;
        }

        public RubyObject()
        {

        }

        public RubyObject(IntPtr Pointer, int Type = 0x01)
        {
            this.Pointer = Pointer;
            Internal.rb_gv_set("$ptr" + Pointer.ToString(), Pointer);
        }

        public void Free()
        {
            AssertUndisposed();
            Internal.rb_gv_set("$ptr" + Pointer.ToString(), Internal.QNil);
            this.Pointer = Internal.QNil;
            this.Freed = true;
        }

        public void AssertUndisposed()
        {
            if (Freed) throw new Exception("This object has already been freed!");
        }

        public override string ToString()
        {
            AssertUndisposed();
            return this.GetType().ToString();
        }

        public T Convert<T>()
        {
            AssertUndisposed();
            return (T) Activator.CreateInstance(typeof(T), this.Pointer);
        }

        public void Print()
        {
            AssertUndisposed();
            Internal.PrintObject(this);
        }

        public void InstanceEval(string Code)
        {
            AssertUndisposed();
            Internal.rb_obj_instance_eval(1, new IntPtr[1] { new RubyString(Code).Pointer }, this.Pointer);
        }

        public bool IsNil()
        {
            AssertUndisposed();
            return Pointer == Internal.QNil;
        }
    }
}
