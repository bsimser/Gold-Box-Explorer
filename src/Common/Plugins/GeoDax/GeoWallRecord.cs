namespace GoldBoxExplorer.Lib.Plugins.GeoDax
{
    public class GeoWallRecord
    {
        public const int ndoor = 1;
        public const int ndoor_locked = 2;
        public const int ndoor_wizard = 3;
        public const int edoor = 4;
        public const int edoor_locked = 8;
        public const int edoor_wizard = 12;
        public const int sdoor = 16;
        public const int sdoor_locked = 32;
        public const int sdoor_wizard = 48;
        public const int wdoor = 64;
        public const int wdoor_locked = 128;
        public const int wdoor_wizard = 192;

        public int Row { get; set; }
        public int Column { get; set; }
        public int North { get; set; }
        public int East { get; set; }
        public int South { get; set; }
        public int West { get; set; }
        public byte Event { get; set; }
        public int Door { get; set; }
    }
}