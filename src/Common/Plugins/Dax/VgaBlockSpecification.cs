namespace GoldBoxExplorer.Lib.Plugins.Dax
{
    public class VgaBlockSpecification : IFileBlockSpecification
    {
        public bool IsSatisfiedBy(FileBlockParameters parameters)
        {
            var data = parameters.Data;

            if (data.Length < 20)
                return false;

            int height = data[0];
            int width = data[1];

            var widthPx = width*8;
            var heightPx = height;

            if (widthPx < 1 || heightPx < 1 || widthPx > 320 || heightPx > 200)
                return false;

            var chunkCount = data[6] + (data[7] << 8);
            var chunkSize = (widthPx * heightPx);
            if ((chunkCount * chunkSize) > data.Length)
                return false;

            int clrBase = data[8];
            int clrCount = data[9];
            var clrs = EgaVgaPalette.ExtractPalette(data, clrCount, clrBase, 10);

            return clrs != null;
        }
    }
}