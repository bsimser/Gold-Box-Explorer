using System.IO;
using System.Text;
using System.Drawing;
using System.Collections.Generic;

namespace GoldBoxExplorer.Lib.Plugins.Text
{
    public class TextFilePlugin : IPlugin
    {
        public bool IsSatisifedBy(string path)
        {
            var windowSize = File.ReadAllBytes(path).Length;
            var isText = false;

            if (windowSize > 0)
            {
                isText = true;

                using (var fileStream = File.OpenRead(path))
                {
                    var encoding = Encoding.Default;
                    var rawData = new byte[windowSize];
                    var text = new char[windowSize];

                    // Read raw bytes
                    var rawLength = fileStream.Read(rawData, 0, rawData.Length);
                    fileStream.Seek(0, SeekOrigin.Begin);

                    // Detect encoding correctly (from Rick Strahl's blog)
                    // http://www.west-wind.com/weblog/posts/2007/Nov/28/Detecting-Text-Encoding-for-StreamReader
                    if (rawData[0] == 0xef && rawData[1] == 0xbb && rawData[2] == 0xbf)
                    {
                        encoding = Encoding.UTF8;
                    }
                    else if (rawData[0] == 0xfe && rawData[1] == 0xff)
                    {
                        encoding = Encoding.Unicode;
                    }
                    else if (rawData[0] == 0 && rawData[1] == 0 && rawData[2] == 0xfe && rawData[3] == 0xff)
                    {
                        encoding = Encoding.UTF32;
                    }
                    else if (rawData[0] == 0x2b && rawData[1] == 0x2f && rawData[2] == 0x76)
                    {
                        encoding = Encoding.UTF7;
                    }

                    // Read text and detect the encoding
                    using (var streamReader = new StreamReader(fileStream))
                    {
                        streamReader.Read(text, 0, text.Length);
                    }

                    using (var memoryStream = new MemoryStream())
                    {
                        using (var streamWriter = new StreamWriter(memoryStream, encoding))
                        {
                            // Write the text to a buffer
                            streamWriter.Write(text);
                            streamWriter.Flush();

                            // Get the buffer from the memory stream for comparision
                            var memoryBuffer = memoryStream.GetBuffer();

                            // Compare only bytes read
                            for (var i = 0; i < rawLength && isText; i++)
                            {
                                isText = rawData[i] == memoryBuffer[i];
                            }
                        }
                    }
                }
            }

            return isText;
        }

        public IPlugin CreateUsing(PluginParameter args)
        {
            var file = new TextFile(args.Filename);
            Viewer = new TextFileViewer(file, args.ContainerWidth);
            return this;
        }

        public IGoldBoxViewer Viewer { get; set; }

        public bool IsImageFile() { return false; }
        public IEnumerable<Bitmap> GetBitmaps()
        {
            return null;
        }
        public IList<int> GetBitmapIds() { return null; }
    }
}