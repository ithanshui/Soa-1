using System;
using Framework.Soa.Common;

namespace Framework.Soa.Agent
{
    public static class ServiceHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TRequest"></typeparam>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="contract"></param>
        /// <param name="serviceUrl"></param>
        /// <param name="serviceToken"></param>
        /// <param name="httpMethod"></param>
        /// <returns></returns>
        public static TResponse GetResponse<TRequest, TResponse>(this Contract<TRequest, TResponse> contract, string serviceUrl, string serviceToken, string httpMethod = "POST")
            where TRequest : Request
            where TResponse : Response
        {
            IAgent agent = AgentFactory.CreateAgent(serviceUrl, httpMethod);

            agent.Request(contract, serviceToken);

            return contract.Response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TRequest"></typeparam>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="contract"></param>
        /// <param name="svcUrl"></param>
        /// <param name="serviceToken"></param>
        /// <param name="httpMethod"></param>
        /// <returns></returns>
        public static TResponse Call<TRequest, TResponse>(TRequest request, string svcUrl, string serviceToken, string httpMethod = "POST")
            where TRequest : Request
            where TResponse : Response
        {
            IAgent agent = AgentFactory.CreateAgent(svcUrl, httpMethod);

            return agent.Call<TRequest, TResponse>(svcUrl, request, serviceToken);
        }
    }
}