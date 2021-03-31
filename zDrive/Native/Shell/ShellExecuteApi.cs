using System.Runtime.InteropServices;

namespace zDrive.Native.Shell
{
    /// <summary>
    /// https://docs.microsoft.com/en-us/windows/win32/api/shellapi/nf-shellapi-shellexecuteexa
    /// </summary>
    public static class ShellExecuteApi
    {
        private const string Shell32 = "shell32.dll";

        /// <summary>
        /// Opens properties windows for specified path.
        /// </summary>
        public static bool ShowFileProperties(string fileName)
        {
            var info = ShellExecuteInfo.Initialize();
            info.Verb = "properties";
            info.File = fileName;
            info.ShowFlags = ShellSw.ShowDefault;
            info.SeeMask = ShellSeeMask.InvokeIdList;
            return ShellExecuteEx(ref info);
        }

        /// <summary>
        /// Performs an operation on a specified file.
        /// https://docs.microsoft.com/en-us/windows/win32/api/shellapi/nf-shellapi-shellexecuteexa
        /// </summary>
        [DllImport(Shell32, CharSet = CharSet.Auto)]
        internal static extern bool ShellExecuteEx(ref ShellExecuteInfo lpExecInfo);
    }
}
