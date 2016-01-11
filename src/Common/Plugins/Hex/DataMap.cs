using System;
using System.Collections;

namespace GoldBoxExplorer.Lib.Plugins.Hex
{
    internal class DataMap : ICollection, IEnumerable
    {
        internal int _version;

        public DataMap()
        {
            SyncRoot = new object();
        }

        public DataMap(IEnumerable collection)
        {
            SyncRoot = new object();
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }

            foreach (DataBlock item in collection)
            {
                AddLast(item);
            }
        }

        public DataBlock FirstBlock { get; internal set; }

        public void CopyTo(Array array, int index)
        {
            var blockArray = array as DataBlock[];
            for (var block = FirstBlock; block != null; block = block.NextBlock)
            {
                if (blockArray != null) blockArray[index++] = block;
            }
        }

        public int Count { get; internal set; }

        public bool IsSynchronized
        {
            get { return false; }
        }

        public object SyncRoot { get; private set; }

        public IEnumerator GetEnumerator()
        {
            return new Enumerator(this);
        }

        public void AddAfter(DataBlock block, DataBlock newBlock)
        {
            AddAfterInternal(block, newBlock);
        }

        public void AddBefore(DataBlock block, DataBlock newBlock)
        {
            AddBeforeInternal(block, newBlock);
        }

        public void AddFirst(DataBlock block)
        {
            if (FirstBlock == null)
            {
                AddBlockToEmptyMap(block);
            }
            else
            {
                AddBeforeInternal(FirstBlock, block);
            }
        }

        public void AddLast(DataBlock block)
        {
            if (FirstBlock == null)
            {
                AddBlockToEmptyMap(block);
            }
            else
            {
                AddAfterInternal(GetLastBlock(), block);
            }
        }

        public void Remove(DataBlock block)
        {
            RemoveInternal(block);
        }

        public void RemoveFirst()
        {
            if (FirstBlock == null)
            {
                throw new InvalidOperationException("The collection is empty.");
            }
            RemoveInternal(FirstBlock);
        }

        public void RemoveLast()
        {
            if (FirstBlock == null)
            {
                throw new InvalidOperationException("The collection is empty.");
            }
            RemoveInternal(GetLastBlock());
        }

        public DataBlock Replace(DataBlock block, DataBlock newBlock)
        {
            AddAfterInternal(block, newBlock);
            RemoveInternal(block);
            return newBlock;
        }

        public void Clear()
        {
            DataBlock block = FirstBlock;
            while (block != null)
            {
                DataBlock nextBlock = block.NextBlock;
                InvalidateBlock(block);
                block = nextBlock;
            }
            FirstBlock = null;
            Count = 0;
            _version++;
        }

        private void AddAfterInternal(DataBlock block, DataBlock newBlock)
        {
            newBlock.PreviousBlock = block;
            newBlock.NextBlock = block.NextBlock;
            newBlock.Map = this;

            if (block.NextBlock != null)
            {
                block.NextBlock.PreviousBlock = newBlock;
            }
            block.NextBlock = newBlock;

            _version++;
            Count++;
        }

        private void AddBeforeInternal(DataBlock block, DataBlock newBlock)
        {
            newBlock.NextBlock = block;
            newBlock.PreviousBlock = block.PreviousBlock;
            newBlock.Map = this;

            if (block.PreviousBlock != null)
            {
                block.PreviousBlock.NextBlock = newBlock;
            }
            block.PreviousBlock = newBlock;

            if (FirstBlock == block)
            {
                FirstBlock = newBlock;
            }
            _version++;
            Count++;
        }

        private void RemoveInternal(DataBlock block)
        {
            DataBlock previousBlock = block.PreviousBlock;
            DataBlock nextBlock = block.NextBlock;

            if (previousBlock != null)
            {
                previousBlock.NextBlock = nextBlock;
            }

            if (nextBlock != null)
            {
                nextBlock.PreviousBlock = previousBlock;
            }

            if (FirstBlock == block)
            {
                FirstBlock = nextBlock;
            }

            InvalidateBlock(block);

            Count--;
            _version++;
        }

        private DataBlock GetLastBlock()
        {
            DataBlock lastBlock = null;
            for (var block = FirstBlock; block != null; block = block.NextBlock)
            {
                lastBlock = block;
            }
            return lastBlock;
        }

        private static void InvalidateBlock(DataBlock block)
        {
            block.Map = null;
            block.NextBlock = null;
            block.PreviousBlock = null;
        }

        private void AddBlockToEmptyMap(DataBlock block)
        {
            block.Map = this;
            block.NextBlock = null;
            block.PreviousBlock = null;

            FirstBlock = block;
            _version++;
            Count++;
        }

        internal class Enumerator : IEnumerator, IDisposable
        {
            private readonly DataMap _map;
            private readonly int _version;
            private DataBlock _current;
            private int _index;

            internal Enumerator(DataMap map)
            {
                _map = map;
                _version = map._version;
                _current = null;
                _index = -1;
            }

            public void Dispose()
            {
            }

            object IEnumerator.Current
            {
                get
                {
                    if (_index < 0 || _index > _map.Count)
                    {
                        throw new InvalidOperationException(
                            "Enumerator is positioned before the first element or after the last element of the collection.");
                    }
                    return _current;
                }
            }

            public bool MoveNext()
            {
                if (_version != _map._version)
                {
                    throw new InvalidOperationException("Collection was modified after the enumerator was instantiated.");
                }

                if (_index >= _map.Count)
                {
                    return false;
                }

                if (++_index == 0)
                {
                    _current = _map.FirstBlock;
                }
                else
                {
                    _current = _current.NextBlock;
                }

                return (_index < _map.Count);
            }

            void IEnumerator.Reset()
            {
                if (_version != _map._version)
                {
                    throw new InvalidOperationException("Collection was modified after the enumerator was instantiated.");
                }

                _index = -1;
                _current = null;
            }
        }
    }
}