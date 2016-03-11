namespace Framework.Soa.Common
{
    /// <summary>
    /// 泛型契约基类
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public abstract class Contract<TRequest, TResponse> : IContract<TRequest, TResponse>
         where TRequest : Request
         where TResponse : Response
    {
        /// <summary>
        /// 
        /// </summary>
        public TRequest Request { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public TResponse Response { get; set; }
    }
}