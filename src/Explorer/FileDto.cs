using System;
using System.IO;
using System.Runtime.InteropServices;

namespace GoldBoxExplorer
{
    public class FileDto
    {
        public string Name { get; set; }
        public string LastWrite { get; set; }
        public string Type { get; set; }
        public string Size { get; set; }

        [StructLayout(LayoutKind.Sequential)]
        public struct SHFILEINFO
        {
            public IntPtr hIcon;
            public IntPtr iIcon;
            public uint dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;
        };

        public const uint SHGFIICON = 0x100;
        public const uint SHGFI_LARGEICON = 0x0; // 'Large icon
        public const uint SHGFI_SMALLICON = 0x1; // 'Small icon
        public const uint SHGFI_TYPENAME = 0x000000400;
        public const uint SHGFI_USEFILEATTRIBUTES = 0x000000010;
        public const uint FILE_ATTRIBUTE_NORMAL = 0x80;

		[DllImport("shell32.dll")]
        public static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbSizeFileInfo, uint uFlags);

        public FileDto(FileInfo info)
        {
            Name = info.Name.ToUpper();
            LastWrite = FormatDate(info.LastWriteTime);
            Type = FormatType(info);
            Size = FormatSize(info.Length);
        }

        private static string FormatSize(long length)
        {
            const int scale = 1024;
            var orders = new[] {"GB", "MB", "KB", "Bytes"};
            var max = (long) Math.Pow(scale, orders.Length - 1);

            foreach (string order in orders)
            {
                if (length > max)
                    return String.Format("{0:##.##} {1}", Decimal.Divide(length, max), order);
                max /= scale;
            }

            return "0 Bytes";
        }

        private static string FormatType(FileInfo fileInfo)
        {
            var info = new SHFILEINFO();
            const uint flags = (uint) SHGFI_TYPENAME | SHGFI_USEFILEATTRIBUTES;
            SHGetFileInfo(fileInfo.FullName, FILE_ATTRIBUTE_NORMAL, ref info, (uint) Marshal.SizeOf(info), flags);
            return info.szTypeName;
        }

        private static string FormatDate(DateTime dateTime)
        {
            return string.Format("{0} {1}", dateTime.ToShortDateString(), dateTime.ToShortTimeString());
        }
    }
}