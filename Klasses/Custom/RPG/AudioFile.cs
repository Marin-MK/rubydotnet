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
            return c;
        }

        public AudioFile()
        {

        }

        public AudioFile(IntPtr Pointer)
            : base(Pointer)
        {

        }

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
    }
}
