using System;

namespace Framework.Soa.Common
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ContractAttribute : Attribute
    {
        public string ContractUrl { get; set; }

        public string ContractDesc { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contractUrl"></param>
        /// <param name="contractDesc"></param>
        public ContractAttribute(string contractUrl, string contractDesc = "")
        {
            this.ContractUrl = contractUrl;
            this.ContractDesc = contractDesc;
        }
    }
}