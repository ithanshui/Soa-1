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

        public MessageDTO()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageText"></param>
        /// <param name="messageCode"></param>
        public MessageDTO(string messageText, string messageCode)
        {
            this.MessageText = messageText;
            this.MessageCode = messageCode;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageText"></param>
        /// <param name="messageCode"></param>
        /// <param name="responseTime"></param>
        public MessageDTO(string messageText, string messageCode, TimeSpan responseTime)
            : this(messageText, messageCode)
        {
            this.ResponseTime = responseTime;
        }
    }
}