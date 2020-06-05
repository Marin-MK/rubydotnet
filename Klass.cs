using System;
using System.Collections.Generic;
using System.Text;

namespace RubyDotNET
{
    public class Klass
    {
        public string KlassName;
        public IntPtr Pointer;
        // Isn't used for anything, but methods must be added to this list to keep them from being GC'd.
        // Is guaranteed to throw an error if you try to use it more than once without this workaround.
        private List<Internal.RubyMethod> Methods = new List<Internal.RubyMethod>();

        public Klass()
        {

        }

        public Klass(string KlassName, IntPtr Pointer)
        {
            this.KlassName = KlassName;
            this.Pointer = Pointer;
        }

        public void DefineMethod(string Name, Internal.RubyMethod Method, int Count = -2)
        {
            Internal.rb_define_method(Pointer, Name, Method, Count);
            Methods.Add(Method);
        }

        public void DefineClassMethod(string Name, Internal.RubyMethod Method, int Count = -2)
        {
            Internal.rb_define_singleton_method(Pointer, Name, Method, Count);
            Methods.Add(Method);
        }

        public void DefineConstant(string Name, IntPtr Value)
        {
            Internal.rb_define_const(this.Pointer, Name, Value);
        }
    }
}
