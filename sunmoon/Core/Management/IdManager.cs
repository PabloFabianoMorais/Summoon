using System.Threading;

namespace sunmoon.Core.Management
{
    /// <summary>
    /// Gerencia os ids
    /// </summary>
    public static class IdManager
    {
        private static long _nextId = 0;

        /// <summary>
        /// Gera um id único
        /// </summary>
        /// <returns>Retorna um id</returns>
        public static long GenerateId()
        {
            return Interlocked.Increment(ref _nextId);
        }
    }
}