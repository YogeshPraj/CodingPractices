using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.PS.LibraryCodeSamples.Exceptions
{
    /// <summary>
    /// https://blogs.msdn.microsoft.com/kcwalina/2005/03/16/design-guidelines-update-exception-throwing/
    /// </summary>
    public class ExceptionSample3
    {
        public async Task DoSomething_Before(DbConnection conn)
        {
            try
            {
                // some db related code
                
                conn.Close();
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.GetType().FullName);
                Console.WriteLine(ex.Message);
            }
        }

        public async Task DoSomething_After(DbConnection conn)
        {
            // some db related code

            if (conn.State != ConnectionState.Closed)
            {
                conn.Close();
            }
        }
    }
}
