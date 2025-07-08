using System.Threading;

namespace sunmoon.Core.Management
{
    public static class IdManager
    {
        private static long _nextId = 0;

        public static long GenerateId()
        {
            return Interlocked.Increment(ref _nextId);
        }
    }
}