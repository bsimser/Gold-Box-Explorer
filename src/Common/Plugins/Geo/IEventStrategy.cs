using System.IO;

namespace GoldBoxExplorer.Lib.Plugins.Geo
{
    internal interface IEventStrategy
    {
        Event LoadEvent(BinaryReader reader);
    }
}