using System.Collections.Generic;
using System.IO;

namespace GoldBoxExplorer.Common.Frua.Frua
{
    public class FruaGlbFile : GoldBoxFile
    {
        private readonly string _fullPath;
        private string _statusMessage = "FRUA GLB File";

        public FruaGlbFile(string fullPath)
        {
            _fullPath = fullPath;
            ReadHeader();
        }

        private void ReadHeader()
        {
            using (var stream = new FileStream(_fullPath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = new BinaryReader(stream))
                {
                    var header = new FruaCommonGlbFileHeader
                                  {
                                      HeaderText = new string(reader.ReadChars(4)),
                                      FileSize = reader.ReadUInt32(),
                                  };

                    var filename = Path.GetFileName(_fullPath).ToUpper();

                    if (filename.Equals("STRG.GLB"))
                    {
                        ReadStrgGlb(reader);
                    }

                    if (filename.Equals("SCRIPT.GLB"))
                    {
                        ReadScriptGlb(reader);
                    }

                    if (filename.Equals("MONST.GLB"))
                    {
                        ReadMonsterGlb(reader);
                    }
                }
            }
        }

        private void ReadMonsterGlb(BinaryReader reader)
        {
            var recordCount = reader.ReadByte();
            _statusMessage = string.Format("MONST.GLB {0} records", recordCount);
        }

        private void ReadStrgGlb(BinaryReader reader)
        {
            var sectionsInFile = reader.ReadByte();
            _statusMessage = string.Format("STRG.GLB {0} sections", sectionsInFile);
            reader.BaseStream.Seek(66, SeekOrigin.Begin);
            for (int s = 0; s < sectionsInFile - 1; s++)
            {
                var numberOfRecords = reader.ReadUInt16();
                for (int i = 0; i < numberOfRecords; i++)
                {
                    // TODO this is broken
                    /*
                                var pointer = reader.ReadByte();
                                var category = reader.ReadByte();
                                var name = reader.ReadBytes(12);
                                 */
                }
            }
        }

        private void ReadScriptGlb(BinaryReader reader)
        {
            var scriptHeader = new FruaSecondGlbHeader
                {
                    EntryCount = reader.ReadUInt32(),
                    DataText = new string(reader.ReadChars(4))
                };

            _statusMessage = string.Format("SCRIPT.GLB {0} entries", scriptHeader.EntryCount);
            var offsets = new List<uint>();

            for (int i = 0; i < scriptHeader.EntryCount; i++)
            {
                offsets.Add(reader.ReadUInt32());
            }

            // TODO not complete see SCRIPT.TXT in HACKDOCS
            foreach (var offset in offsets)
            {
                reader.BaseStream.Seek(offset, SeekOrigin.Begin);
            }
        }

        public override IList<byte> GetBytes()
        {
            return File.ReadAllBytes(_fullPath);
        }

        public override string GetStatusMessage()
        {
            return _statusMessage;
        }
    }
}