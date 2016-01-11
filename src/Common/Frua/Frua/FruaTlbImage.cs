namespace GoldBoxExplorer.Common.Frua.Frua
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