namespace Framework.Soa.Common
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public interface IContract<out TRequest, out TResponse>
         where TRequest : IRequest
         where TResponse : IResponse
    {
        /// <summary>
        /// 请求
        /// </summary>
        TRequest Request { get; }

        /// <summary>
        /// 响应
        /// </summary>
        TResponse Response { get; }
    }
}