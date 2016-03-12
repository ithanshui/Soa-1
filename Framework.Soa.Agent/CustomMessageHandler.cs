using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;

namespace Framework.Soa.Agent
{
    /// <summary>
    /// 
    /// </summary>
    public class CustomMessageHandler : DelegatingHandler
    {
        #region Private Fields
        /// <summary>
        /// 
        /// </summary>
        private readonly string serviceToken;
        #endregion

        #region Ctor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceToken"></param>
        public CustomMessageHandler(string serviceToken)
        {
            this.serviceToken = serviceToken;
            this.InnerHandler = new HttpClientHandler();
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Add("ServiceToken", this.serviceToken);
            return base.SendAsync(request, cancellationToken);
        }
    }
}