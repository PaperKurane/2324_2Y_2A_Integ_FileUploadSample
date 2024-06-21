using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace _2324_2Y_2A_Integ_FileUploadSample
{
    internal class FileHelper
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CloseHandle(IntPtr hObject);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr CreateFile(
            string lpFileName,
            uint dwDesiredAccess,
            uint dwShareMode,
            IntPtr lpSecurityAttributes,
            uint dwCreationDisposition,
            uint dwFlagsAndAttributes,
            IntPtr hTemplateFile);

        private const uint GENERIC_WRITE = 0x40000000;
        private const uint FILE_SHARE_READ = 0x00000001;
        private const uint OPEN_EXISTING = 3;

        public static bool IsFileInUse(string filePath)
        {
            IntPtr handle = CreateFile(filePath, GENERIC_WRITE, FILE_SHARE_READ, IntPtr.Zero, OPEN_EXISTING, 0, IntPtr.Zero);

            if (handle != IntPtr.Zero)
            {
                CloseHandle(handle);
                return false;
            }

            return true;
        }

        public static void ForceCloseFileHandle(string filePath)
        {
            if (IsFileInUse(filePath))
            {
                System.Windows.MessageBox.Show($"The file '{filePath}' is in use by another process. Please close it and try again.");
                throw new IOException($"The file '{filePath}' is in use by another process.");
            }
        }
    }
}
