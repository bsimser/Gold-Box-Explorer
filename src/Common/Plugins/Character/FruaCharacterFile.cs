using System.Collections.Generic;
using System.IO;
using GoldBoxExplorer.Lib.Plugins.Dax;
namespace GoldBoxExplorer.Lib.Plugins.Character
{
    public class FruaCharacterFile : DaxFile
    {
        public List<byte> _fileData;
        private readonly string _path;
        public bool variableRecords = false;
        public int variableRecordStart = 0;
        public int variableRecordLength = 0x11D;
        public int variableRecordNumber = 0;
        public string formatType = "poolrad";
        public bool daxFile = false;
        public int _blockId;
        public List<int> _blockIds = new List<int>();
        public List<byte[]> _daxBlocks = new List<byte[]>();
        

            public FruaCharacterFile(string path)
            : base(path, false)
        {
            _path = path;
            
            var fileExtension = Path.GetExtension(_path);
            int fileSize = GetBytes().Count;

            if (Path.GetFileName(_path).Contains("ITEMS")) // TODO use filename instead of path
            {
                formatType = "item template";
                variableRecords = true;
                variableRecordStart = 2;
                variableRecordLength = 16;
                variableRecordNumber = (fileSize - variableRecordStart) / variableRecordLength;
                
            }

            if (fileExtension.ToUpper() == ".ITM")
            {
                formatType = "item";
                variableRecords = true;
                variableRecordStart = 0;
                variableRecordLength = 63;
                variableRecordNumber = fileSize / variableRecordLength;
            }
            if (fileExtension.ToUpper() == ".SPC")
            {
                formatType = "spc";
                variableRecords = true;
                variableRecordStart = 0;
                variableRecordLength = 9;
                variableRecordNumber = fileSize / variableRecordLength;
            }
            if (fileExtension.ToUpper() == ".DAX")
            {
                daxFile = true;
                load(path);
                if (Path.GetFileName(_path).Contains("CHA")) // TODO use filename instead of path
                {
                    formatType = "poolrad";
                    variableRecordLength = 0x11D;
                }
                if (Path.GetFileName(_path).Contains("SPC")) // TODO use filename instead of path
                {
                    formatType = "daxspc";
                    variableRecords = true;
                    variableRecordStart = 0;
                    variableRecordLength = 9;
                    variableRecordNumber = fileSize / variableRecordLength;
                }
                if (Path.GetFileName(_path).Contains("ITM") || Path.GetFileName(_path).Contains("ITEM"))
                {

                    // item
                    formatType = "daxitem";
                    variableRecordStart = 0;
                    variableRecordLength = 63;
                }
                _blockId = -1;
                ProcessBlocks();
            }
  //          LoadCharacter();
        }

        public override IList<byte> GetBytes()
        {
            return File.ReadAllBytes(_path);
        }

        protected override sealed void ProcessBlocks()
        {
            foreach (var block in Blocks)
            {
                _blockIds.Add(block.Id);
                _daxBlocks.Add(block.Data);
            }
        }


        public FruaCharacter LoadCharacter()
        {
            var ch = new FruaCharacter(formatType);
            return ch;
        }
    }
}