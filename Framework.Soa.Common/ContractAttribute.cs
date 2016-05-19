using System;

namespace Framework.Soa.Common
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false)]
    public class ContractAttribute : Attribute
    {
        public string ContractUrl { get; private set; }

        public string ContractDesc { get; private set; }

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