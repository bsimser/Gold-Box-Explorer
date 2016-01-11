using System;

namespace GoldBoxExplorer.Lib.Plugins.Tbl
{
    public class FruaTlbFileHeader
    {
        public string FileType { get; set; }
        public UInt32 FileSize { get; set; }
        public UInt16 Entries { get; set; }
        public byte Unused { get; set; }
        public bool HasTable { get; set; }
        public string ContentType { get; set; }
    }
}