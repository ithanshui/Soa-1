namespace Framework.Soa.Common
{
    /// <summary>
    /// 响应基类
    /// </summary>
    public class Response : IResponse
    {
        public MessageDTO Message { get; set; }
    }
}
