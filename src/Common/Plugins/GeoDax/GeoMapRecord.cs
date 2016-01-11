using System.Collections.Generic;

namespace GoldBoxExplorer.Lib.Plugins.GeoDax
{
    public class GeoMapRecord
    {
        public string Name { get; private set; }
        public IList<GeoWallRecord> Walls { get; private set; }
        public int DaxId { get; private set; }

        public GeoMapRecord(string name, IList<GeoWallRecord> walls, int daxId)
        {
            Name = name;
            Walls = walls;
            DaxId = daxId;
        }
    }
}