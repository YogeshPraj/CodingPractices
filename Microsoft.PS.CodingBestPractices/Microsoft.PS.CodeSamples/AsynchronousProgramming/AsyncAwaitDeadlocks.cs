using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.PS.CodeSamples.AsynchronousProgramming
{
    internal class AsyncAwaitDeadlocks
    {
        ActionResult ActionAsync()
        {
            // DEADLOCK: this blocks on the async task
            var data = GetDataAsync().Result;

            return Ok(data);
        }

        private async Task<string> GetDataAsync()
        {
            // a very simple async method
            var result = await MyWebService.GetDataAsync();
            return result.ToString();
        }
    }

    internal class ActionResult
    {

    }

    internal class Ok
    {
        private string _result;
        Ok(string result)
        {
            _result = result;
        }
    }
}
