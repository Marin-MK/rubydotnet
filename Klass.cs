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
        // Is guaranteed to throw an error if you try to import more than once without this workaround.
        private List<Internal.RubyMethod> Methods = new List<Internal.RubyMethod>();

        public Klass()
        {

        }

        public Klass(string KlassName, IntPtr Pointer)
        {
            this.KlassName = KlassName;
            this.Pointer = Pointer;
        }

        public void DefineMethod(string Name, Internal.RubyMethod Method)
        {
            Internal.rb_define_method(Pointer, Name, Method, -1);
            Methods.Add(Method);
        }

        public void DefineClassMethod(string Name, Internal.RubyMethod Method)
        {
            Internal.rb_define_singleton_method(Pointer, Name, Method, -1);
            Methods.Add(Method);
        }

        public IntPtr GetIVar(string VarName)
        {
            return Internal.rb_ivar_get(this.Pointer, Internal.rb_intern(VarName));
        }

        public void SetIVar(string VarName, RubyObject obj)
        {
            Internal.rb_ivar_set(this.Pointer, Internal.rb_intern(VarName), obj.Pointer);
        }
    }
}
