using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace RubyDotNET
{
    public class AudioFile : RubyObject
    {
        public static Class CreateClass()
        {
            Class c = new Class("AudioFile", null, Internal.GetKlass("RPG"));
            c.DefineMethod("name", name);
            c.DefineMethod("volume", volume);
            c.DefineMethod("pitch", pitch);
            return c;
        }

        public AudioFile()
        {

        }

        public AudioFile(IntPtr Pointer)
            : base(Pointer)
        {

        }

        #region Ruby
        public static IntPtr name(int Argc, IntPtr[] Argv, IntPtr Self)
        {
            return Internal.rb_ivar_get(Self, Internal.rb_intern("@name"));
        }

        public static IntPtr volume(int Argc, IntPtr[] Argv, IntPtr Self)
        {
            return Internal.rb_ivar_get(Self, Internal.rb_intern("@volume"));
        }

        public static IntPtr pitch(int Argc, IntPtr[] Argv, IntPtr Self)
        {
            return Internal.rb_ivar_get(Self, Internal.rb_intern("@pitch"));
        }
        #endregion

        #region C#
        public RubyString GetName()
        {
            return new RubyString(GetIVar("@name"));
        }

        public int GetVolume()
        {
            return (int) Internal.NUM2LONG(GetIVar("@volume"));
        }

        public int GetPitch()
        {
            return (int) Internal.NUM2LONG(GetIVar("@pitch"));
        }
        #endregion
    }
}
