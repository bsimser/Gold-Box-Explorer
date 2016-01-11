using System;

namespace GoldBoxExplorer.Lib.Plugins.Tbl
{
    public class FruaTlbColorTableHeader
    {
        public UInt16 ColorCyclingValue { get; set; }
        public UInt16 FirstPaletteColorUsed { get; set; }
        public UInt16 NumColorsInPalette { get; set; }
        public byte NumColorCyclingRanges { get; set; }
        public byte Magic { get; set; }
    }
}