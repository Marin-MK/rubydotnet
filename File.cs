using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace rubydotnet
{
    public static partial class Ruby
    {
        public class File : Object
        {
            public new static string KlassName = "File";
            public new static Class Class { get => (Class) GetKlass(KlassName); }

            public File(IntPtr Pointer) : base(Pointer, true) { }

            public File(string Name, string Mode = "r") : this(rb_file_open(Name, Mode)) { Pin(); }

            public bool EOF
            {
                get => Funcall("eof?") == True;
            }
            public bool Closed
            {
                get => Funcall("closed?") == True;
            }
            public Integer LineNo
            {
                get => Funcall("lineno").Convert<Integer>();
                set => Funcall("lineno=", value);
            }
            public Integer Pos
            {
                get => Funcall("pos").Convert<Integer>();
                set => Funcall("pos=", value);
            }
            public Integer PID
            {
                get => Funcall("pid").Convert<Integer>();
            }

            public String Read()
            {
                return Funcall("read").Convert<String>();
            }
            public String Read(Integer Length)
            {
                return Funcall("read", Length).Convert<String>();
            }
            public Object GetByte()
            {
                return Funcall("getbyte");
            }
            public Object GetC()
            {
                return Funcall("getc");
            }
            public Object PRead(Integer MaxLength, Integer Offset)
            {
                return Funcall("pread", MaxLength, Offset);
            }
            public Object ReadByte()
            {
                return Funcall("readbyte");
            }
            public Object ReadChar()
            {
                return Funcall("readchar");
            }
            public Object Write(String String)
            {
                return Funcall("write", String);
            }
            public Object PutC(Integer Obj)
            {
                return Funcall("putc", Obj);
            }
            public Object PutC(String Obj)
            {
                return Funcall("putc", Obj);
            }
            public Object Puts(Object Obj)
            {
                return Funcall("puts", Obj);
            }
            public Object PWrite(String String, Integer Offset)
            {
                return Funcall("pwrite", String, Offset);
            }
            public Object Rewind()
            {
                return Funcall("rewind");
            }
            public Object CloseRead()
            {
                return Funcall("close_read");
            }
            public Object CloseWrite()
            {
                return Funcall("close_write");
            }
            public Object Close()
            {
                Unpin();
                return Funcall("close");
            }

            public static String AbsolutePath(String Path)
            {
                return Class.Funcall("absolute_path", Path).Convert<String>();
            }
            public static bool IsAbsolutePath(String Path)
            {
                return Class.Funcall("absolute_path?", Path) == True;
            }
            public static Time ATime(String Filename)
            {
                return Class.Funcall("atime", Filename).Convert<Time>();
            }
            public static String BaseName(String Filename)
            {
                return Class.Funcall("basename", Filename).Convert<String>();
            }
            public static String BaseName(String Filename, String Suffix)
            {
                return Class.Funcall("basename", Filename, Suffix).Convert<String>();
            }
            public static Time CTime(String Filename)
            {
                return Class.Funcall("ctime", Filename).Convert<Time>();
            }
            public static Integer Delete(params String[] Filenames)
            {
                return Class.Funcall("basename", Filenames).Convert<Integer>();
            }
            public static bool IsDirectory(String Filename)
            {
                return Class.Funcall("directory?", Filename) == True;
            }
            public static bool Exist(String Filename)
            {
                return Class.Funcall("exist?", Filename) == True;
            }
            public static String ExpandPath(String Filename)
            {
                return Class.Funcall("expand_path", Filename).Convert<String>();
            }
            public static String ExpandPath(String Filename, String DirString)
            {
                return Class.Funcall("expand_path", Filename, DirString).Convert<String>();
            }
            public static String ExtName(String Filename)
            {
                return Class.Funcall("extname", Filename).Convert<String>();
            }
            public static bool IsFile(String Filename)
            {
                return Class.Funcall("file?", Filename) == True;
            }
            public static String Join(params String[] Strings)
            {
                return Class.Funcall("join", Strings).Convert<String>();
            }
            public static Time MTime(String Filename)
            {
                return Class.Funcall("mtime", Filename).Convert<Time>();
            }
            public static Object Rename(String OldName, String NewName)
            {
                return Class.Funcall("rename", OldName, NewName);
            }
            public static Integer Size(String Filename)
            {
                return Class.Funcall("size", Filename).Convert<Integer>();
            }
            public static Array Split(String Filename)
            {
                return Class.Funcall("split", Filename).Convert<Array>();
            }
            public static Object Truncate(String Filename, Integer Length)
            {
                return Class.Funcall("truncate", Filename, Length);
            }
        }
    }
}
