using System;
using System.Collections.Generic;
using System.Text;

namespace RubyDotNET
{
    public class Module : Klass
    {
        public Module(string Name, IntPtr Pointer)
            : base(Name, Pointer) { }

        public Module(string KlassName, Klass ParentModule = null)
        {
            this.KlassName = KlassName;
            if (ParentModule != null)
            {
                this.Pointer = Internal.rb_define_module_under(ParentModule.Pointer, KlassName);
            }
            else
            {
                this.Pointer = Internal.rb_define_module(KlassName);
            }
            Internal.Klasses.Add(this.Pointer, this);
        }
    }
}
