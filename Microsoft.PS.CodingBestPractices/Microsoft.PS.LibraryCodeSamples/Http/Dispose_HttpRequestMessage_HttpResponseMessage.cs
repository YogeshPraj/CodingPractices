using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Microsoft.PS.LibraryCodeSamples.Http
{
    public class Dispose_HttpRequestMessage_HttpResponseMessage : IDisposable
    {
        HttpClient _httpClient = null;

        public Dispose_HttpRequestMessage_HttpResponseMessage()
        {
            _httpClient = new HttpClient();
        }

        public void Dispose()
        {
            // TODO: implement diposable pattern.
            _httpClient.Dispose();
        }

        public async Task<Result> Sample()
        {
            using (var requestMessage = new HttpRequestMessage(HttpMethod.Post, "Some uri"))
            {
                using (var response = await _httpClient.SendAsync(requestMessage))
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var objResult = new Result
                    {
                        Content = responseContent,
                        StatusCode = response.StatusCode
                    };

                    return objResult;
                }
            }
        }
    }

    public class Result
    {
        public string Content { get; set; }

        public HttpStatusCode StatusCode { get; set; }
    }
}
