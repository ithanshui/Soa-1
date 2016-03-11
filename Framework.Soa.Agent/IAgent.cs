using Framework.Soa.Common;

namespace Framework.Soa.Agent
{
    /// <summary>
    /// 
    /// </summary>
    public interface IAgent
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TRequest"></typeparam>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="contract"></param>
        /// <param name="serviceToken"></param>
        void Request<TRequest, TResponse>(Contract<TRequest, TResponse> contract, string serviceToken)
            where TRequest : Request
            where TResponse : Response;
    }
}