using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.PS.LibraryCodeSamples.Exceptions
{
    /// <summary>
    /// https://blogs.msdn.microsoft.com/kcwalina/2005/03/16/design-guidelines-update-exception-throwing/
    /// </summary>
    public class ExceptionSample2
    {
        public async Task DoSomething_Before(string uri)
        {
            try
            {
                HttpClient client = new HttpClient();

                var response = await client.PostAsync(new Uri(uri), new StringContent("some payload"));
            }
            catch (UriFormatException uriEx)
            {
                // Log Uri is invalid.
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task DoSomething_After(string uri)
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

            try
            {
                HttpClient client = new HttpClient();

                var response = await client.PostAsync(new Uri("some uri"), new StringContent("some payload"));
            }
            catch (Exception ex)
            {
                // logging
                throw;
            }
        }
    }
}
