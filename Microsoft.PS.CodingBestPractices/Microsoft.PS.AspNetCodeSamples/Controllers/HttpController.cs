using System;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Web.Http;

namespace Microsoft.PS.AspNetCodeSamples.Controllers
{
    public class HttpController : ApiController
    {
        private Uri _endpoint = new Uri("https://api.chucknorris.io/jokes/random");
        private static HttpClient _httpClient = new HttpClient();

        [HttpGet]
        [Route("api/v1/http/newhttpclient")]
        public async Task<IHttpActionResult> ExecuteGetWithNewHttpClient()
        {
            var ip = Dns.GetHostAddresses(_endpoint.Host)[0];
            for (int i = 0; i < 5; i++)  
            {
                using (var client = new HttpClient())
                {
                    var response = await client.GetStringAsync(_endpoint);
                }
            }
            var result = IPGlobalProperties.GetIPGlobalProperties()
                                .GetActiveTcpConnections();
            var count = 0;
            foreach(var connection in result)
            {
                if (ip.ToString().Equals(connection.RemoteEndPoint.Address.ToString()))
                {
                    count++;
                }
            }
            return Ok(string.Format("Active TCP Connections post method completion : {0}",count.ToString()));
        }

        [HttpGet]
        [Route("api/v1/http/singlehttpclient")]
        public async Task<IHttpActionResult> ExecuteGetWithStaticHttpClient()
        {
            var ip = Dns.GetHostAddresses(_endpoint.Host)[0];
            for (int i = 0; i < 5; i++)
            {
                var response = await _httpClient.GetStringAsync(_endpoint);
            }
            var result = IPGlobalProperties.GetIPGlobalProperties()
                                .GetActiveTcpConnections();
            var count = 0;
            foreach (var connection in result)
            {
                if (ip.ToString().Equals(connection.RemoteEndPoint.Address.ToString()))
                {
                    count++;
                }
            }
            return Ok(string.Format("Active TCP Connections post method completion : {0}", count.ToString()));
        }                
    }
}
