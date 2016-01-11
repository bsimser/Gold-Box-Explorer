using System;

namespace GoldBoxExplorer.Lib.Plugins.Hex
{
    internal sealed class FileDataBlock : DataBlock
    {
        long _length;
        long _fileOffset;

        public FileDataBlock(long fileOffset, long length)
        {
            _fileOffset = fileOffset;
            _length = length;
        }

        public long FileOffset
        {
            get
            {
                return _fileOffset;
            }
        }

        public override long Length
        {
            get
            {
                return _length;
            }
        }

        public void SetFileOffset(long value)
        {
            _fileOffset = value;
        }

        public void RemoveBytesFromEnd(long count)
        {
            if (count > _length)
            {
                throw new ArgumentOutOfRangeException("count");
            }

            _length -= count;
        }

        public void RemoveBytesFromStart(long count)
        {
            if (count > _length)
            {
                throw new ArgumentOutOfRangeException("count");
            }

            _fileOffset += count;
            _length -= count;
        }
    }
}