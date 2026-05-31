using System.Collections.Concurrent;

namespace Cupid.Utils
{
    public class CupidConnectionTracker
    {
        private readonly ConcurrentDictionary<string, string> _connections = new();

        public void Add(string connectionID, string username)
        {
            _connections.TryAdd(connectionID, username);
        }

        public bool TryGetUsername(string connectionId, out string? username)
        {
            return _connections.TryGetValue(connectionId, out username);
        }

        public bool TryRemove(string connectionId, out string? username)
        {
            return _connections.TryRemove(connectionId, out username);
        }
    }
}
