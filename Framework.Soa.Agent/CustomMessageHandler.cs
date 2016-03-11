using System.Threading.Tasks;
using System.Net.Http;

namespace Framework.Soa.Agent
{
    /// <summary>
    /// 
    /// </summary>
    public class CustomMessageHandler : DelegatingHandler
    {
        private string serviceToken;

        public CustomMessageHandler(string serviceToken)
        {
            this.serviceToken = serviceToken;
            this.InnerHandler = new HttpClientHandler();
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            request.Headers.Add("ServiceToken", this.serviceToken);
            return base.SendAsync(request, cancellationToken);
        }
    }
}