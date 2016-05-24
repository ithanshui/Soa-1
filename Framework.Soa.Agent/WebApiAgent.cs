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
        private string serviceUrl;

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
        /// <param name="serviceUrl"></param>
        /// <param name="httpMethod"></param>
        public WebApiAgent(string serviceUrl, string httpMethod = "POST")
        {
            this.serviceUrl = serviceUrl;
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
            this.serviceUrl = this.serviceUrl.TrimEnd('/');

            ContractAttribute contractAttribute = Helper.GetContractAttribute(contract.GetType());

            this.serviceUrl += "/" + contractAttribute.ContractUrl.TrimStart('/');

            return new Uri(serviceUrl);
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
                            contract.Response = Helper.DeserializeObject<TResponse>(strTask.Result);
                        else
                        {
                            contract.Response = (TResponse)Activator.CreateInstance(typeof(TResponse));
                            contract.Response.Message = new MessageDTO("0", "");
                        }
                    }
                    catch (Exception ex)
                    {
                        contract.Response = (TResponse)Activator.CreateInstance(typeof(TResponse));
                        contract.Response.Message = new MessageDTO("0", ex.Message);
                    }
                }
            }

            sw.Stop();

            contract.Response.Message.ResponseTime = sw.Elapsed;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TRequest"></typeparam>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="serviceUrl"></param>
        /// <param name="request"></param>
        /// <param name="serviceToken"></param>
        /// <returns></returns>
        public TResponse Call<TRequest, TResponse>(string serviceUrl, TRequest request, string serviceToken)
            where TRequest : Request
            where TResponse : Response
        {
            Stopwatch sw = new Stopwatch();

            sw.Start();

            TResponse response = default(TResponse);

            CustomMessageHandler handler = new CustomMessageHandler(serviceToken);

            using (HttpClient client = new HttpClient(handler))
            {
                using (StringContent content = new StringContent(Helper.SerializeObject(request), Encoding.UTF8, mediaType))
                {
                    try
                    {
                        Task<HttpResponseMessage> responseTask = GetResponseAsync(new Uri(serviceUrl), content, client);

                        Task<string> strTask = responseTask.Result.Content.ReadAsStringAsync();

                        if (responseTask.Result.IsSuccessStatusCode)
                        {
                            response = Helper.DeserializeObject<TResponse>(strTask.Result);
                            response.Message = new MessageDTO("1", "");
                        }
                        else
                        {
                            response = (TResponse)Activator.CreateInstance(typeof(TResponse));
                            response.Message = new MessageDTO("0", "");
                        }
                    }
                    catch (Exception ex)
                    {
                        response = (TResponse)Activator.CreateInstance(typeof(TResponse));
                        response.Message = new MessageDTO("0", ex.Message);
                    }
                }
            }

            sw.Stop();

            response.Message.ResponseTime = sw.Elapsed;

            return response;
        }
    }
}