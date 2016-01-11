using System.IO;

namespace GoldBoxExplorer.Lib.Plugins.Dax
{
    public class MonoBlockSpecification : IFileBlockSpecification
    {
        public bool IsSatisfiedBy(FileBlockParameters parameters)
        {
            return (parameters.Data.Length%8) == 0 &&
                   (Path.GetFileName(parameters.Name).ToUpper().StartsWith("8X8"));
        }
    }
}