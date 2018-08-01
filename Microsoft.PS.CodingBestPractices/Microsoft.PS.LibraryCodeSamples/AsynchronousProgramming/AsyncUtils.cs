using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.IT.GRM.Core.Helper
{
    /// <summary>
    /// Calling async method in sync method
    /// </summary>
    public static class AsyncUtils
    {
        private static readonly Lazy<TaskFactory> _taskFactory = new Lazy<TaskFactory>(() => new TaskFactory(CancellationToken.None, TaskCreationOptions.None, TaskContinuationOptions.None, TaskScheduler.Default));

        /// <summary>
        /// Run sync.
        /// </summary>
        public static TResult RunSync<TResult>(Func<Task<TResult>> func)
        {
            return _taskFactory.Value.StartNew(func).Unwrap().GetAwaiter().GetResult();
        }

        /// <summary>
        /// Run sync.
        /// </summary>
        public static void RunSync(Func<Task> func)
        {
            _taskFactory.Value.StartNew(func).Unwrap().GetAwaiter().GetResult();
        }
    }

    public class AsyncUtilsUsage
    {
        public void GetData()
        {
            Dictionary<long, bool> asyncMethodResponseInSyncMethod = 
                AsyncUtils.RunSync(() => CheckProjectRequestDateChangedAsync(
                            new long[] { 1, 2, 3, 4 },
                            DateTime.Now,
                            DateTime.Now.AddDays(1)));
        }

        /// <summary>
        /// This is somewhere in different class
        /// </summary>
        /// <returns></returns>
        public async Task<Dictionary<long,bool>> CheckProjectRequestDateChangedAsync(long[] projectIds, DateTime newStartDate, DateTime newEndDate)
        {
            var result = new Dictionary<long, bool>();
            foreach (var projectId in projectIds)
            {
                result.Add(projectId, true);
            }

            return await Task.FromResult(result);
        }
    }
}
