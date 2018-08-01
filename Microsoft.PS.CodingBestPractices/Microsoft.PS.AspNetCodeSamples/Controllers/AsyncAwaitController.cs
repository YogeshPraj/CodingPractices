using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Microsoft.PS.AspNetCodeSamples.Controllers
{    
    public class AsyncAwaitController : ApiController
    {
        #region Async Await Deadlock
        [HttpGet]
        [Route("api/v1/asyncawait/deadlocksample")]
        public async Task<IHttpActionResult> AsyncAwaitWithResultDeadlock()
        {
            //below code causes a deadlock
            //1. The GetDataAsync.Result will run when the task returned by GetDataAsync has completed
            //2. The continuation of the awaited method MyWebService.GetDataAsync is queued to the main thread 
            //   and will complete when continuation if run.
            //3. However, the queued continuation never runs since the main thread is blocked

            var deadLockedResponse = GetDataAsync().Result;
            return Ok(deadLockedResponse);
        }

        [HttpGet]
        [Route("api/v1/asyncawait/deadlockresolution")]
        public async Task<IHttpActionResult> AsyncAwaitWithoutResult()
        {
            var deadLockedResponse = await GetDataAsync();
            return Ok(deadLockedResponse);
        }
        #endregion

        #region ConfigureAwait
        [HttpGet]
        [Route("api/v1/asyncawait/withconfigureawait")]
        public async Task<IHttpActionResult> AsyncBehaviorWithConfigureAwait([FromUri]bool continueOnContext)
        {
            //The difference comes in AFTER the async operation is completed:

            //When we specify ConfigureAwait(true) we get to come back to the Request Context that called the async method -this does some housekeeping and syncs up the HttpContext so it's not null when we continue on
            //When we specify ConfigureAwait(false) we just continue on without going back to the Request Context, therefore HttpContext is null

            var beforeRunningValue = HttpContext.Current != null;
            var whileRunningValue = await ExecuteExampleAsync(continueOnContext).ConfigureAwait(continueOnContext);
            var afterRunningValue = HttpContext.Current != null;

            return Ok(new
            {
                ContinueOnCapturedContext = continueOnContext,
                BeforeRunningValue = beforeRunningValue,
                WhileRunningValue = whileRunningValue,
                AfterRunningValue = afterRunningValue,
                SameBeforeAndAfter = beforeRunningValue == afterRunningValue
            });
        }
        #endregion

        #region Async Cancellation
        [HttpGet]
        [Route("api/v1/asyncawait/taskwithcancellation")]
        public async Task<IHttpActionResult> TaskWithCancellationSupport([FromUri] bool shouldCancelTask)
        {
            var cancellationTokenSource = new CancellationTokenSource();
            if (shouldCancelTask)
            {
                cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(1));
            }
            var task = Task.Run(() => LongRunningOperationWithCancellation(1, 2, cancellationTokenSource.Token), cancellationTokenSource.Token);
            try
            {
                var result = await task;
            }
            catch (OperationCanceledException)
            {
                return Ok("Cancelled");
            }
            return Ok("Completed");
        }
        #endregion

        #region Private Methods
        private async Task<bool> ExecuteExampleAsync(bool continueOnContext)
        {
            return await Task.Delay(TimeSpan.FromMilliseconds(10)).ContinueWith((task) =>
            {
                var hasHttpContext = HttpContext.Current != null;
                return hasHttpContext;
            }).ConfigureAwait(continueOnContext);
        }

        private async Task<string> GetDataAsync()
        {
            // a very simple async method
            var result = await MyWebService.GetDataAsync();
            return result.ToString();
        }

        private int LongRunningOperationWithCancellation(int a, int b, CancellationToken cancellationToken)
        {
            string someString = string.Empty;
            for (int i = 0; i < 200000; i++)
            {
                someString += "a";
                if (i % 1000 == 0)
                    cancellationToken.ThrowIfCancellationRequested();
            }

            return a + b;
        }
        #endregion

    }

    internal class MyWebService
    {
        internal static async Task<string> GetDataAsync()
        {
            await Task.Delay(2000);
            return "{\"Response\":\"Hello World\"}";
        } 
    }
    
}
