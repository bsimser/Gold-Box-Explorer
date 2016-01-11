using System.Collections.Generic;
using System.IO;

namespace GoldBoxExplorer.Lib.Plugins.Dax
{
    public abstract class DaxFile : GoldBoxFile
    {
        List<DaxFileBlock> _blocks;

        protected DaxFile(string path, bool autoLoad = true)
        {
            if (autoLoad)
                load(path);
        }

        public string FileName { get; private set; }

        public DaxFileBlock getBlockById(int id)
        {
            foreach (var b in _blocks)
            {
                if (b.Id == id)
                    return b;
            }
            return null;
        }
        public IEnumerable<DaxFileBlock> Blocks
        {
            get { return _blocks.AsReadOnly(); }
        }

        public override IList<byte> GetBytes()
        {
            var bytes = new List<byte>();
            foreach (var block in Blocks)
            {
                bytes.AddRange(block.Data);
            }
            return bytes;
        }

        protected abstract void ProcessBlocks();

        public void load(string file)
        {
            FileName = file;

            using (var stream = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                var reader = new BinaryReader(stream);
                var dataOffset = reader.ReadInt16() + 2;
                var headers = new List<DaxFileHeaderEntry>();
                const int headerEntrySize = 9;

                for (var i = 0; i < ((dataOffset - 2) / headerEntrySize); i++)
                {
                    var dhe = new DaxFileHeaderEntry
                                  {
                                      Id = reader.ReadByte(),
                                      Offset = reader.ReadInt32(),
                                      RawSize = reader.ReadUInt16(),
                                      CompressedSize = reader.ReadUInt16()
                                  };
                    headers.Add(dhe);
                }

                _blocks = new List<DaxFileBlock>(headers.Count);

                foreach (var dhe in headers)
                {
                    byte[] compressedBytes;
                    if (dhe.RawSize <= 0)
                    {
                        reader.BaseStream.Seek(dataOffset + dhe.Offset, SeekOrigin.Begin);
                        compressedBytes = reader.ReadBytes(dhe.CompressedSize);
                        _blocks.Add(new DaxFileBlock(FileName, dhe.Id, compressedBytes));
                    }
                    else
                    {
                        var raw = new byte[dhe.RawSize];
                        reader.BaseStream.Seek(dataOffset + dhe.Offset, SeekOrigin.Begin);
                        compressedBytes = reader.ReadBytes(dhe.CompressedSize);
                        if (compressedBytes.Length == 0)
                            continue;
                        decodeCompressedBytes(dhe.CompressedSize, raw, compressedBytes);
                        _blocks.Add(new DaxFileBlock(FileName, dhe.Id, raw));
                    }
                }
            }
        }

        private static void decodeCompressedBytes(int dataLength, IList<byte> outputPtr, IList<byte> inputPtr)
        {
            var inputIndex = 0;
            var outputIndex = 0;

                do
                {
                    var runLength = (sbyte)inputPtr[inputIndex];

                    if (runLength >= 0)
                    {
                        for (var i = 0; i <= runLength; i++)
                        {

                            if (inputIndex + i + 1 >= inputPtr.Count) return; // some files seem to have faulty encoding and run over the limit
                            outputPtr[outputIndex + i] = inputPtr[inputIndex + i + 1];
                        }

                        inputIndex += runLength + 2;
                        outputIndex += runLength + 1;
                    }
                    else
                    {
                        runLength = (sbyte)(-runLength);

                        for (var i = 0; i < runLength; i++)
                        {
                            outputPtr[outputIndex + i] = inputPtr[inputIndex + 1];
                        }

                        inputIndex += 2;
                        outputIndex += runLength;
                    }
                } while (inputIndex < dataLength);

        }
    }
}