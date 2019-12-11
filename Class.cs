using System;
using System.Collections.Generic;
using System.Text;

namespace RubyDotNET
{
    public class Class : Klass
    {
        public Class(string Name, IntPtr Pointer)
            : base(Name, Pointer) { }

        public Class(string KlassName, Klass InheritedClass = null, Klass ParentClass = null)
        {
            this.KlassName = KlassName;
            Klass inherited = InheritedClass ?? Internal.cObject;
            if (ParentClass != null)
            {
                this.Pointer = Internal.rb_define_class_under(ParentClass.Pointer, KlassName, inherited.Pointer);
            }
            else
            {
                this.Pointer = Internal.rb_define_class(KlassName, inherited.Pointer);
            }
            Internal.Klasses.Add(this.Pointer, this);
        }
    }
}
