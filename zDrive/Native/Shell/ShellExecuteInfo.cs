using System;
using System.Runtime.InteropServices;

namespace zDrive.Native.Shell
{
    /// <summary>
    /// Contains information used by ShellExecuteEx.
    /// https://docs.microsoft.com/en-us/windows/win32/api/shellapi/ns-shellapi-shellexecuteinfoa
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct ShellExecuteInfo
    {
        public uint Size;
        public ShellSeeMask SeeMask;
        public IntPtr Handle;

        [MarshalAs(UnmanagedType.LPTStr)]
        public string Verb;

        [MarshalAs(UnmanagedType.LPTStr)]
        public string File;

        [MarshalAs(UnmanagedType.LPTStr)]
        public string Parameters;

        [MarshalAs(UnmanagedType.LPTStr)]
        public string Directory;

        public ShellSw ShowFlags;
        public IntPtr HInstApp;
        public IntPtr IdList;

        [MarshalAs(UnmanagedType.LPTStr)]
        public string Class;

        public IntPtr KeyClass;
        public uint HotKey;
        public IntPtr Icon;
        public IntPtr Process;

        public static ShellExecuteInfo Initialize() =>
            new() { Size = (uint)Marshal.SizeOf(typeof(ShellExecuteInfo)) };
    }
}
