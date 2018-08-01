using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Microsoft.PS.LibraryCodeSamples.Http
{
    /// <summary>
    /// Reference url: http://faithlife.codes/blog/2017/03/usage-guidelines-for-httpclient/
    /// </summary>
    public class HttpClient_Before
    {
        public HttpClient_Before()
        {

        }

        public async Task<string> GetUserProfile(string alias)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetStringAsync("some get profile url");
            }

            return "some response";
        }
    }

    public class HttpClient_After : IDisposable
    {
        HttpClient _client;

        public HttpClient_After()
        {
            _client = new HttpClient();
        }

        public void Dispose()
        {
            // Follow disposable pattern
            // dispose http client 
            _client.Dispose();
        }

        public async Task<string> GetUserProfile(string alias)
        {
            var response = await _client.GetStringAsync("some get profile url");
            return "some response";
        }
    }

    public class HttpClient_With_ConnectionLeaseTimeout
    {
        HttpClient _client;

        public HttpClient_With_ConnectionLeaseTimeout()
        {
            _client = new HttpClient();
            ServicePointManager.FindServicePoint(new Uri("some uri")).ConnectionLeaseTimeout = 60 * 1000;
        }

        public async Task<string> GetUserProfile(string alias)
        {
            var response = await _client.GetStringAsync("some get profile url");
            return "some response";
        }
    }

}
