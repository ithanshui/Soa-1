using System;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Diagnostics;

using Framework.Soa.Common;

namespace Framework.Soa.Agent
{
    /// <summary>
    /// 
    /// </summary>
    public class WebApiAgent : IAgent
    {
        #region Private Fields
        /// <summary>
        /// 
        /// </summary>
        private string svcUrl;

        /// <summary>
        /// 
        /// </summary>
        private readonly string httpMethod;

        /// <summary>
        /// 
        /// </summary>
        private readonly string mediaType = "application/json";
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="svcUrl"></param>
        /// <param name="httpMethod"></param>
        public WebApiAgent(string svcUrl, string httpMethod = "POST")
        {
            this.svcUrl = svcUrl;
            this.httpMethod = httpMethod;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TRequest"></typeparam>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="contract"></param>
        /// <returns></returns>
        private Uri GetUri<TRequest, TResponse>(Contract<TRequest, TResponse> contract)
            where TRequest : Request
            where TResponse : Response
        {
            this.svcUrl = this.svcUrl.TrimEnd('/');

            ContractAttribute contractAttribute = Helper.GetContractAttribute(contract.GetType());

            this.svcUrl += "/" + contractAttribute.ContractUrl.TrimStart('/');

            return new Uri(svcUrl);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="content"></param>
        /// <param name="client"></param>
        /// <returns></returns>
        private Task<HttpResponseMessage> GetResponseAsync(Uri uri, StringContent content, HttpClient client)
        {
            Task<HttpResponseMessage> task = null;

            switch (this.httpMethod.ToUpper())
            {
                case "GET":
                    task = client.GetAsync(uri);
                    break;
                case "POST":
                    task = client.PostAsync(uri, content);
                    break;
                case "PUT":
                    task = client.PutAsync(uri, content);
                    break;
                case "DELETE":
                    task = client.DeleteAsync(uri);
                    break;
                default:
                    task = client.PostAsync(uri, content);
                    break;
            }

            return task;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TRequest"></typeparam>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="contract"></param>
        /// <param name="serviceToken"></param>
        public void Request<TRequest, TResponse>(Contract<TRequest, TResponse> contract, string serviceToken)
            where TRequest : Request
            where TResponse : Response
        {
            Stopwatch sw = new Stopwatch();

            sw.Start();

            CustomMessageHandler handler = new CustomMessageHandler(serviceToken);

            using (HttpClient client = new HttpClient(handler))
            {
                using (StringContent content = new StringContent(Helper.SerializeObject(contract.Request), Encoding.UTF8, mediaType))
                {
                    try
                    {
                        Task<HttpResponseMessage> responseTask = GetResponseAsync(GetUri(contract), content, client);

                        Task<string> strTask = responseTask.Result.Content.ReadAsStringAsync();

                        if (responseTask.Result.IsSuccessStatusCode)
                        {
                            contract.Response = Helper.DeserializeObject<TResponse>(strTask.Result);
                        }
                    }
                    catch (Exception ex)
                    {
                        contract.Response = (TResponse)Activator.CreateInstance(typeof(TResponse));
                        contract.Response.Message = new MessageDTO()
                        {
                            MessageCode = "0",
                            MessageText = ex.Message
                        };
                    }
                }
            }

            sw.Stop();

            contract.Response.Message.ResponseTime = sw.Elapsed;
        }
    }
}