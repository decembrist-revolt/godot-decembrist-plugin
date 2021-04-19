using System;
using System.Threading;
using VoidTask = System.Threading.Tasks.Task;

namespace Decembrist.Utils
{
    public static class Locks
    {
        
        /// <summary>
        /// Synchronize callback invoke (for example once per time)
        /// </summary>
        /// <param name="semaphore">Mutex</param>
        /// <param name="callback">Block to invoke</param>
        /// <returns>Exec task</returns>
        public static async VoidTask Synchronize(this SemaphoreSlim semaphore, Func<VoidTask> callback)
        {
            await semaphore.WaitAsync();
            try
            {
                await callback();
            }
            finally
            {
                semaphore.Release();
            }
        }
        
        /// <summary>
        /// Get single mutex
        /// </summary>
        /// <returns>Mutex</returns>
        public static SemaphoreSlim NewMutex() => new SemaphoreSlim(1,1);
    }
}