using System;
using System.Diagnostics;
using System.Collections.Concurrent;

using Framework.Soa.Common;
using Newtonsoft.Json;

namespace Framework.Soa.Agent
{
    public class Helper
    {
        private static ConcurrentDictionary<string, ContractAttribute> contractDictionary = new ConcurrentDictionary<string, ContractAttribute>();

        private static ContractAttribute GetContractAttributeFromType(Type contractType)
        {
            ContractAttribute contract = contractType.GetCustomAttributes(false)[0] as ContractAttribute;

            if (contract == null) { throw new ArgumentNullException("ContractAttribute"); }

            return contract;
        }

        public static ContractAttribute GetContractAttribute(Type contractType)
        {
            ContractAttribute contract = null;

            bool result = contractDictionary.TryGetValue(contractType.Name, out contract);

            if (!result)
            {
                contract = GetContractAttributeFromType(contractType);

                contractDictionary.TryAdd(contractType.Name, contract);
            }

            return contract;
        }

        public static T DeserializeObject<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }

        public static string SerializeObject<T>(T t)
        {
            return JsonConvert.SerializeObject(t);
        }
    }
}
