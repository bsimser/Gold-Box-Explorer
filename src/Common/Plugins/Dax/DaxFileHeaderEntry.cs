namespace GoldBoxExplorer.Lib.Plugins.Dax
{
    public class DaxFileHeaderEntry
    {
        public int Id { get; set; }
        public int Offset { get; set; }
        public int RawSize { get; set; }
        public int CompressedSize { get; set; }
    }
}