namespace GoldBoxExplorer.Lib.Plugins.Tbl
{
    public class FruaTlbImage
    {
        public FruaTlbImageHeader Header { get; private set; }

        public FruaTlbImage()
        {
            Header = new FruaTlbImageHeader();
        }
    }
}