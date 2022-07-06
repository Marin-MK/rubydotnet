using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rubydotnet;

public static partial class Ruby
{
    public static class File
    {
        public static IntPtr Open(string Filename, string Mode)
        {
            return Funcall(GetConst(Object.Class, "File"), "open", String.ToPtr(Filename), String.ToPtr(Mode));
        }

        public static IntPtr Read(IntPtr File)
        {
            return Funcall(File, "read");
        }

        public static IntPtr Close(IntPtr File)
        {
            return Funcall(File, "close");
        }
    }
}
