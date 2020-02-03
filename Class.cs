using System;
using System.Reflection;

namespace RubyDotNET
{
    public class Class : Klass
    {
        public Class()
        {

        }

        public Class(string Name, IntPtr Pointer)
            : base(Name, Pointer) { }

        public Class(string KlassName, Klass InheritedClass = null, Klass ParentClass = null)
        {
            this.KlassName = KlassName;
            Klass inherited = InheritedClass ?? Internal.rb_cObject;
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
