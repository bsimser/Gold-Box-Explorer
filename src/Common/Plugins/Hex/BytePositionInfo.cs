namespace GoldBoxExplorer.Lib.Plugins.Hex
{
    internal struct BytePositionInfo
    {
        private readonly int _characterPosition;
        private readonly long _index;

        public BytePositionInfo(long index, int characterPosition)
        {
            _index = index;
            _characterPosition = characterPosition;
        }

        public int CharacterPosition
        {
            get { return _characterPosition; }
        }

        public long Index
        {
            get { return _index; }
        }
    }
}