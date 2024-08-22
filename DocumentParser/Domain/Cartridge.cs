using System;

namespace DocumentParser.Domain
{
    public class Cartridge<T>
    {
        private T _item;

        public bool IsFilled => _item != null;

        public void TryAdd(T item)
        {
            if (IsFilled)
            {
                throw new InvalidOperationException("Cannot add more than one item");
            }

            _item = item;
        }

        
        public T TryGetItem()
        {
            if (!IsFilled)
            {
                throw new InvalidOperationException("No item available");
            }

            return _item;
        }
        public T TryRemove()
        {
            if (!IsFilled)
            {
                throw new InvalidOperationException("Cannot remove an item");
            }

            T item = _item;
            _item = default;

            return item;
        }
    }
}