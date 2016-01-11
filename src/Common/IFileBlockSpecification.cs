using GoldBoxExplorer.Lib.Plugins.Dax;

namespace GoldBoxExplorer.Lib
{
    public interface IFileBlockSpecification
    {
        bool IsSatisfiedBy(FileBlockParameters parameters);
    }
}