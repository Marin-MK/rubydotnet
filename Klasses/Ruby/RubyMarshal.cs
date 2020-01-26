using System;
using System.Collections.Generic;
using System.Text;

namespace RubyDotNET
{
    public class RubyMarshal : RubyObject
    {
        public static T Load<T>(RubyObject Object)
        {
            Object.AssertUndisposed();
            IntPtr ptr = Internal.rb_marshal_load(Object.Pointer);
            return (T) Activator.CreateInstance(typeof(T), ptr);
        }
    }
}
