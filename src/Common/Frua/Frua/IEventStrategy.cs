using System.IO;

namespace GoldBoxExplorer.Common.Frua.Frua
{
    internal interface IEventStrategy
    {
        Event LoadEvent(BinaryReader reader);
    }
}