using System;
using Framework.Soa.Common;
using System.Collections.Concurrent;

using Newtonsoft.Json;

namespace Framework.Soa.Agent
{
    public class Helper
    {
        private static ConcurrentDictionary<string, ContractAttribute> contractDictionary = new ConcurrentDictionary<string, ContractAttribute>();

        private static ContractAttribute GetContractAttributeFromType(Type contractType)
        {
            ContractAttribute contract = null;

            object[] objects = contractType.GetCustomAttributes(true);

            foreach (object @object in objects)
            {
                if (@object.GetType().Name == "ContractAttribute")
                {
                    contract = @object as ContractAttribute;

                    break;
                }
            }

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
