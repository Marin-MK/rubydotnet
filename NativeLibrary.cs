using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace rubydotnet
{
    public enum Platform
    {
        Unknown,
        Windows,
        Linux,
        MacOS,
        IOS,
        Android
    }

    public class NativeLibrary
    {
        private static Platform? _platform;
        /// <summary>
        /// The current OS.
        /// </summary>
        public static Platform Platform
        {
            get
            {
                if (_platform != null) return (Platform) _platform;
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) _platform = Platform.Windows;
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) _platform = Platform.MacOS;
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) _platform = Platform.Linux;
                else _platform = Platform.Unknown;
                return (Platform) _platform;
            }
        }

        // Windows
        [DllImport("kernel32")]
        static extern IntPtr LoadLibrary(string Filename);

        [DllImport("kernel32")]
        static extern IntPtr GetProcAddress(IntPtr Handle, string FunctionName);

        // Linux
        [DllImport("libdl.so")]
        public static extern IntPtr dlopen(string filename, int flags);

        [DllImport("libdl.so")]
        public static extern IntPtr dlsym(IntPtr Handle, string FunctionName);

        public static List<NativeLibrary> LoadedLibraries = new List<NativeLibrary>();

        public string Name;
        public IntPtr Handle;

        public NativeLibrary(string Library, params string[] PreloadLibraries)
        {
            foreach (string PreloadLibrary in PreloadLibraries)
            {
                if (LoadedLibraries.Find(l => l.Name == PreloadLibrary) != null) continue;
                LoadedLibraries.Add(new NativeLibrary(PreloadLibrary));
            }
            Name = Library;
            if (Platform == Platform.Windows) Handle = LoadLibrary(Library);
            else if (Platform == Platform.Linux) Handle = dlopen(Library, 0x102);
            else if (Platform == Platform.MacOS) throw new Exception("MacOS is not currently supported.");
            else throw new Exception("Platform could not be determined.");
            if (Handle == IntPtr.Zero) throw new Exception($"Failed to load libarary '{Library}'.");
        }

        public TDelegate GetFunction<TDelegate>(string FunctionName)
        {
            IntPtr funcaddr = IntPtr.Zero;
            if (Platform == Platform.Windows) funcaddr = GetProcAddress(Handle, FunctionName);
            else if (Platform == Platform.Linux) funcaddr = dlsym(Handle, FunctionName);
            else if (Platform == Platform.MacOS) throw new Exception("MacOS is not currently supported.");
            else throw new Exception("Platform could not be determined.");
            if (funcaddr == IntPtr.Zero) throw new InvalidEntryPoint(Name, FunctionName);
            return Marshal.GetDelegateForFunctionPointer<TDelegate>(funcaddr);
        }

        public static void IEP(NativeLibrary Library, string FunctionName)
        {
            throw new InvalidEntryPoint(Library.Name, FunctionName);
        }

        public class InvalidEntryPoint : Exception
        {
            public InvalidEntryPoint(string Library, string FunctionName) : base($"No entry point by the name of '{FunctionName}' could be found in '{Library}'.")
            {

            }
        }
    }
}
