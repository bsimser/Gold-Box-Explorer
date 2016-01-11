using System;

namespace GoldBoxExplorer.Lib.Plugins.Hex
{
    public interface IByteProvider
    {
        /// <summary>
        /// Reads a byte from the provider
        /// </summary>
        /// <param name="index">the index of the byte to read</param>
        /// <returns>the byte to read</returns>
        byte ReadByte(long index);
        
        /// <summary>
        /// Writes a byte into the provider
        /// </summary>
        /// <param name="index">the index of the byte to write</param>
        /// <param name="value">the byte to write</param>
        void WriteByte(long index, byte value);
        
        /// <summary>
        /// Inserts bytes into the provider
        /// </summary>
        /// <param name="index"></param>
        /// <param name="bs"></param>
        /// <remarks>This method must raise the LengthChanged event.</remarks>
        void InsertBytes(long index, byte[] bs);
        
        /// <summary>
        /// Returns the total length of bytes the byte provider is providing.
        /// </summary>
        long Length { get; }
        
        /// <summary>
        /// Occurs, when the Length property changed.
        /// </summary>
        event EventHandler LengthChanged;

        /// <summary>
        /// Occurs, when bytes are changed.
        /// </summary>
        event EventHandler Changed;
    }
}