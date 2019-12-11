using System;
using System.Collections.Generic;
using System.Text;

namespace RubyDotNET
{
    public class RubyMarshal : RubyObject
    {
        public static T Load<T>(RubyString String)
        {
            IntPtr ptr = Internal.rb_marshal_load(String.Pointer);
            return (T) Activator.CreateInstance(typeof(T), ptr);
        }
    }
}
