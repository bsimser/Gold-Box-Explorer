using System;

namespace GoldBoxExplorer.Common.Frua.Frua
{
    public class FruaTlbImageHeader
    {
        public UInt16 ImageHeight { get; set; }
        public Int16 VerticalOffset { get; set; }
        public Int16 HorizontalOffset { get; set; }
        public int ImageWidth { get; set; }
        public byte DrawingMethod { get; set; }
    }
}