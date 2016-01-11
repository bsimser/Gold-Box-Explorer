namespace GoldBoxExplorer.Lib.Plugins.Dax
{
    public class DaxFileBlock
    {
        public string File { get; private set; }
        public int Id { get; private set; }
        public byte[] Data { get; set; }

        public DaxFileBlock(string file, int id, byte[] data)
        {
            File = file;
            Id = id;
            Data = data;
        }
    }
}