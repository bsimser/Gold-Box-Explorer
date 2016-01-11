namespace GoldBoxExplorer.Lib
{
    public interface IGoldBlockFileSpecification
    {
        bool IsSatisfiedBy(GoldBoxFileParameters parameters);
    }
}