using System;
using System.IO;

namespace GoldBoxExplorer.Lib.Plugins.Hex
{
    public sealed class DynamicFileByteProvider : IByteProvider, IDisposable
    {
        private const int COPY_BLOCK_SIZE = 4096;

        private DataMap _dataMap;
        private Stream _stream;

        /// <summary>
        ///     Constructs a new <see cref="DynamicFileByteProvider" /> instance.
        /// </summary>
        /// <param name="fileName">The name of the file from which bytes should be provided.</param>
        public DynamicFileByteProvider(string fileName) : this(fileName, false)
        {
        }

        /// <summary>
        ///     Constructs a new <see cref="DynamicFileByteProvider" /> instance.
        /// </summary>
        /// <param name="fileName">The name of the file from which bytes should be provided.</param>
        /// <param name="readOnly">True, opens the file in read-only mode.</param>
        public DynamicFileByteProvider(string fileName, bool readOnly)
        {
            _stream = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            ReInitialize();
        }

        /// <summary>
        ///     Constructs a new <see cref="DynamicFileByteProvider" /> instance.
        /// </summary>
        /// <param name="stream">the stream containing the data.</param>
        /// <remarks>
        ///     The stream must supported seek operations.
        /// </remarks>
        public DynamicFileByteProvider(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");
            if (!stream.CanSeek)
                throw new ArgumentException("stream must supported seek operations(CanSeek)");

            _stream = stream;
            ReInitialize();
        }

        /// <summary>
        ///     See <see cref="IByteProvider.LengthChanged" /> for more information.
        /// </summary>
        public event EventHandler LengthChanged;

        /// <summary>
        ///     See <see cref="IByteProvider.Changed" /> for more information.
        /// </summary>
        public event EventHandler Changed;

        /// <summary>
        ///     See <see cref="IByteProvider.ReadByte" /> for more information.
        /// </summary>
        public byte ReadByte(long index)
        {
            long blockOffset;
            DataBlock block = GetDataBlock(index, out blockOffset);
            var fileBlock = block as FileDataBlock;
            if (fileBlock != null)
            {
                return ReadByteFromFile(fileBlock.FileOffset + index - blockOffset);
            }
            else
            {
                var memoryBlock = (MemoryDataBlock) block;
                return memoryBlock.Data[index - blockOffset];
            }
        }

        /// <summary>
        ///     See <see cref="IByteProvider.WriteByte" /> for more information.
        /// </summary>
        public void WriteByte(long index, byte value)
        {
            try
            {
                // Find the block affected.
                long blockOffset;
                DataBlock block = GetDataBlock(index, out blockOffset);

                // If the byte is already in a memory block, modify it.
                var memoryBlock = block as MemoryDataBlock;
                if (memoryBlock != null)
                {
                    memoryBlock.Data[index - blockOffset] = value;
                    return;
                }

                var fileBlock = (FileDataBlock) block;

                // If the byte changing is the first byte in the block and the previous block is a memory block, extend that.
                if (blockOffset == index && block.PreviousBlock != null)
                {
                    var previousMemoryBlock = block.PreviousBlock as MemoryDataBlock;
                    if (previousMemoryBlock != null)
                    {
                        previousMemoryBlock.AddByteToEnd(value);
                        fileBlock.RemoveBytesFromStart(1);
                        if (fileBlock.Length == 0)
                        {
                            _dataMap.Remove(fileBlock);
                        }
                        return;
                    }
                }

                // If the byte changing is the last byte in the block and the next block is a memory block, extend that.
                if (blockOffset + fileBlock.Length - 1 == index && block.NextBlock != null)
                {
                    var nextMemoryBlock = block.NextBlock as MemoryDataBlock;
                    if (nextMemoryBlock != null)
                    {
                        nextMemoryBlock.AddByteToStart(value);
                        fileBlock.RemoveBytesFromEnd(1);
                        if (fileBlock.Length == 0)
                        {
                            _dataMap.Remove(fileBlock);
                        }
                        return;
                    }
                }

                // Split the block into a prefix and a suffix and place a memory block in-between.
                FileDataBlock prefixBlock = null;
                if (index > blockOffset)
                {
                    prefixBlock = new FileDataBlock(fileBlock.FileOffset, index - blockOffset);
                }

                FileDataBlock suffixBlock = null;
                if (index < blockOffset + fileBlock.Length - 1)
                {
                    suffixBlock = new FileDataBlock(
                        fileBlock.FileOffset + index - blockOffset + 1,
                        fileBlock.Length - (index - blockOffset + 1));
                }

                block = _dataMap.Replace(block, new MemoryDataBlock(value));

                if (prefixBlock != null)
                {
                    _dataMap.AddBefore(block, prefixBlock);
                }

                if (suffixBlock != null)
                {
                    _dataMap.AddAfter(block, suffixBlock);
                }
            }
            finally
            {
                OnChanged(EventArgs.Empty);
            }
        }

        public void InsertBytes(long index, byte[] bs)
        {
            try
            {
                // Find the block affected.
                long blockOffset;
                DataBlock block = GetDataBlock(index, out blockOffset);

                // If the insertion point is in a memory block, just insert it.
                var memoryBlock = block as MemoryDataBlock;
                if (memoryBlock != null)
                {
                    memoryBlock.InsertBytes(index - blockOffset, bs);
                    return;
                }

                var fileBlock = (FileDataBlock) block;

                // If the insertion point is at the start of a file block, and the previous block is a memory block, append it to that block.
                if (blockOffset == index && block.PreviousBlock != null)
                {
                    var previousMemoryBlock = block.PreviousBlock as MemoryDataBlock;
                    if (previousMemoryBlock != null)
                    {
                        previousMemoryBlock.InsertBytes(previousMemoryBlock.Length, bs);
                        return;
                    }
                }

                // Split the block into a prefix and a suffix and place a memory block in-between.
                FileDataBlock prefixBlock = null;
                if (index > blockOffset)
                {
                    prefixBlock = new FileDataBlock(fileBlock.FileOffset, index - blockOffset);
                }

                FileDataBlock suffixBlock = null;
                if (index < blockOffset + fileBlock.Length)
                {
                    suffixBlock = new FileDataBlock(
                        fileBlock.FileOffset + index - blockOffset,
                        fileBlock.Length - (index - blockOffset));
                }

                block = _dataMap.Replace(block, new MemoryDataBlock(bs));

                if (prefixBlock != null)
                {
                    _dataMap.AddBefore(block, prefixBlock);
                }

                if (suffixBlock != null)
                {
                    _dataMap.AddAfter(block, suffixBlock);
                }
            }
            finally
            {
                Length += bs.Length;
                OnLengthChanged(EventArgs.Empty);
                OnChanged(EventArgs.Empty);
            }
        }

        public long Length { get; private set; }

        /// <summary>
        ///     See <see cref="IDisposable.Dispose" /> for more information.
        /// </summary>
        public void Dispose()
        {
            if (_stream != null)
            {
                _stream.Close();
                _stream = null;
            }
            _dataMap = null;
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     See <see cref="Object.Finalize" /> for more information.
        /// </summary>
        ~DynamicFileByteProvider()
        {
            Dispose();
        }

        private void OnLengthChanged(EventArgs e)
        {
            if (LengthChanged != null)
                LengthChanged(this, e);
        }

        private void OnChanged(EventArgs e)
        {
            if (Changed != null)
            {
                Changed(this, e);
            }
        }

        private DataBlock GetDataBlock(long findOffset, out long blockOffset)
        {
            if (findOffset < 0 || findOffset > Length)
            {
                throw new ArgumentOutOfRangeException("findOffset");
            }

            // Iterate over the blocks until the block containing the required offset is encountered.
            blockOffset = 0;
            for (DataBlock block = _dataMap.FirstBlock; block != null; block = block.NextBlock)
            {
                if ((blockOffset <= findOffset && blockOffset + block.Length > findOffset) || block.NextBlock == null)
                {
                    return block;
                }
                blockOffset += block.Length;
            }
            return null;
        }

        private FileDataBlock GetNextFileDataBlock(DataBlock block, long dataOffset, out long nextDataOffset)
        {
            // Iterate over the remaining blocks until a file block is encountered.
            nextDataOffset = dataOffset + block.Length;
            block = block.NextBlock;
            while (block != null)
            {
                var fileBlock = block as FileDataBlock;
                if (fileBlock != null)
                {
                    return fileBlock;
                }
                nextDataOffset += block.Length;
                block = block.NextBlock;
            }
            return null;
        }

        private byte ReadByteFromFile(long fileOffset)
        {
            // Move to the correct position and read the byte.
            if (_stream.Position != fileOffset)
            {
                _stream.Position = fileOffset;
            }
            return (byte) _stream.ReadByte();
        }

        private void MoveFileBlock(FileDataBlock fileBlock, long dataOffset)
        {
            // First, determine whether the next file block needs to move before this one.
            long nextDataOffset;
            FileDataBlock nextFileBlock = GetNextFileDataBlock(fileBlock, dataOffset, out nextDataOffset);
            if (nextFileBlock != null && dataOffset + fileBlock.Length > nextFileBlock.FileOffset)
            {
                // The next block needs to move first, so do that now.
                MoveFileBlock(nextFileBlock, nextDataOffset);
            }

            // Now, move the block.
            if (fileBlock.FileOffset > dataOffset)
            {
                // Move the section to earlier in the file stream (done in chunks starting at the beginning of the section).
                var buffer = new byte[COPY_BLOCK_SIZE];
                for (long relativeOffset = 0; relativeOffset < fileBlock.Length; relativeOffset += buffer.Length)
                {
                    long readOffset = fileBlock.FileOffset + relativeOffset;
                    var bytesToRead = (int) Math.Min(buffer.Length, fileBlock.Length - relativeOffset);
                    _stream.Position = readOffset;
                    _stream.Read(buffer, 0, bytesToRead);

                    long writeOffset = dataOffset + relativeOffset;
                    _stream.Position = writeOffset;
                    _stream.Write(buffer, 0, bytesToRead);
                }
            }
            else
            {
                // Move the section to later in the file stream (done in chunks starting at the end of the section).
                var buffer = new byte[COPY_BLOCK_SIZE];
                for (long relativeOffset = 0; relativeOffset < fileBlock.Length; relativeOffset += buffer.Length)
                {
                    var bytesToRead = (int) Math.Min(buffer.Length, fileBlock.Length - relativeOffset);
                    long readOffset = fileBlock.FileOffset + fileBlock.Length - relativeOffset - bytesToRead;
                    _stream.Position = readOffset;
                    _stream.Read(buffer, 0, bytesToRead);

                    long writeOffset = dataOffset + fileBlock.Length - relativeOffset - bytesToRead;
                    _stream.Position = writeOffset;
                    _stream.Write(buffer, 0, bytesToRead);
                }
            }

            // This block now points to a different position in the file.
            fileBlock.SetFileOffset(dataOffset);
        }

        private void ReInitialize()
        {
            _dataMap = new DataMap();
            _dataMap.AddFirst(new FileDataBlock(0, _stream.Length));
            Length = _stream.Length;
        }
    }
}