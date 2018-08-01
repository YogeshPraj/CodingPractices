using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.ExceptionServices;

namespace Microsoft.PS.LibraryCodeSamples.Exceptions
{
    /// <summary>
    /// https://msdn.microsoft.com/en-us/magazine/mt620018.aspx
    /// In C# 5.0, a mechanism was added that enables 
    /// the throwing of a previously thrown exception 
    /// without losing the stack trace information in the original exception. 
    /// </summary>
    public class ExceptionSample4
    {
        public async Task DoSomething(DbConnection conn)
        {
            Task task = WriteWebRequestSizeAsync("some uri");
            try
            {
                while (!task.Wait(100))
                {
                    Console.Write(".");
                }
            }
            catch (AggregateException exception)
            {
                exception = exception.Flatten();
                ExceptionDispatchInfo.Capture(
                  exception.InnerException).Throw();
            }
        }

        public async Task<bool> WriteWebRequestSizeAsync(string uri)
        {
            if (uri == null)
            {
                // The null URI is unexpected.
                throw new ArgumentNullException("uri", "Uri is required");
            }

            Uri uriObj = null;
            if (!Uri.TryCreate(uri, UriKind.RelativeOrAbsolute, out uriObj))
            {
                throw new UriFormatException("Given uri is invalid.");
            }

            return await Task.FromResult(true);
        }
    }
}
