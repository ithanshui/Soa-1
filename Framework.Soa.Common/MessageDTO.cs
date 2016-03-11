using System;

namespace Framework.Soa.Common
{
    /// <summary>
    /// 消息DTO
    /// </summary>
    public class MessageDTO
    {
        public string MessageText { get; set; }

        public string MessageCode { get; set; }

        public TimeSpan ResponseTime { get; set; }
    }
}
