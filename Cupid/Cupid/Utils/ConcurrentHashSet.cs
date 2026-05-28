namespace Cupid.Utils
{
    public class ConcurrentHashSet<T>
    {
        private readonly HashSet<T> _hashSet = new HashSet<T>();
        private readonly object _lock = new object();

        public bool Add(T item)
        {
            lock (_lock)
            {
                return _hashSet.Add(item);
            }
        }

        public bool Remove(T item)
        {
            lock (_lock)
            {
                return _hashSet.Remove(item);
            }
        }
        public bool Contains(T item)
        {
            lock (_lock)
            {
                return _hashSet.Contains(item);
            }
        }

        public void Clear()
        {
            lock (_lock)
            {
                _hashSet.Clear();
            }
        }

        public int Count
        {
            get
            {
                lock (_lock)
                {
                    return _hashSet.Count;
                }
            }
        }
    }
}
