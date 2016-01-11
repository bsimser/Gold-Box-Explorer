using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoldBoxExplorer.Lib.Plugins.Dax;
//using GoldBoxExplorer.Lib.Plugins.DaxEcl.EclDump;

namespace GoldBoxExplorer.Lib.Plugins.DaxEcl
{
    public class DaxEclFile : DaxFile
    {

        private readonly List<string> _strings = new List<string>();
        public List<EclDump.EclDump> eclDumps = new List<EclDump.EclDump>();

        public DaxEclFile(string path) : base(path)
        {

            ProcessBlocks();
        }

        
        protected override sealed void ProcessBlocks()
        {
            foreach (var block in Blocks)
            {
                eclDumps.Add(new EclDump.EclDump(block.Data, block.Id, FileName));
            }
            foreach (var blockStrings in Blocks.Select(daxFileBlock => DecompressEclText(daxFileBlock.Data)))
            {

                _strings.AddRange(blockStrings);
            }
        }

        private static IEnumerable<string> DecompressEclText(byte[] data)
        {
            var strings = new List<string>();

            for (var i = 0; i < data.Length - 2; i++)
            {
                if (data[i] != 0x80) continue;
                int len = data[i + 1];

                if (i + 1 + len >= data.Length) continue;
                var tdata = new byte[len];

                Array.Copy(data, i + 2, tdata, 0, len);
                var txt = DecompressString(tdata).Trim();
                if (!IsReadableText(txt)) continue;
                strings.Add(txt);
            }

            return strings.AsReadOnly();
        }

        private static bool IsReadableText(string txt)
        {
            var invalidCharArray = new[] { 
                '_', '=', '[', ']', '(', ')', '%', '<', '$', '*',
                '&', '\\', '/', '^', '>',
            };

            var nospace = 0;

            if (txt.Trim().Length <= 3) return false;
            if (Char.IsNumber(txt[0])) return false;
            if (Char.IsPunctuation(txt[0])) return false;

            foreach (var ch in txt)
            {
                if (Char.IsWhiteSpace(ch)) nospace = 0; else nospace++;
                if (nospace > 15) return false;
                if (invalidCharArray.Contains(ch)) return false;
            }

            return true;
        }

        private static string DecompressString(IEnumerable<byte> data)
        {
            var sb = new StringBuilder();
            var state = 1;
            uint lastByte = 0;

            foreach (uint thisByte in data)
            {
                uint curr;
                switch (state)
                {
                    case 1:
                        curr = (thisByte >> 2) & 0x3F;
                        if (curr != 0) sb.Append(InflateChar(curr));
                        state = 2;
                        break;

                    case 2:
                        curr = ((lastByte << 4) | (thisByte >> 4)) & 0x3F;
                        if (curr != 0) sb.Append(InflateChar(curr));
                        state = 3;
                        break;

                    case 3:
                        curr = ((lastByte << 2) | (thisByte >> 6)) & 0x3F;
                        if (curr != 0) sb.Append(InflateChar(curr));

                        curr = thisByte & 0x3F;
                        if (curr != 0) sb.Append(InflateChar(curr));
                        state = 1;
                        break;
                }
                lastByte = thisByte;
            }

            return sb.ToString();
        }

        private static char InflateChar(uint arg0)
        {
            if (arg0 <= 0x1f)
            {
                arg0 += 0x40;
            }

            return (char) arg0;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var s in _strings)
            {
                sb.AppendLine(s);
            }
            return sb.ToString();
        }
    }
}