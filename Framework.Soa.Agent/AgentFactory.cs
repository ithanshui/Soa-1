namespace Framework.Soa.Agent
{
    public class AgentFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceUrl"></param>
        /// <param name="httpMethod"></param>
        /// <param name="svcType"></param>
        /// <returns></returns>
        public static IAgent CreateAgent(string serviceUrl, string httpMethod = "POST", string svcType = "WebApi")
        {
            IAgent agent = null;

            switch (svcType)
            {
                case "WebApi":
                    agent = new WebApiAgent(serviceUrl, httpMethod);
                    break;
            }

            return agent;
        }
    }
}