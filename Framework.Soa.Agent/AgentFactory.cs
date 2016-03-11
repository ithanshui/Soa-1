namespace Framework.Soa.Agent
{
    public class AgentFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="svcUrl"></param>
        /// <param name="httpMethod"></param>
        /// <param name="svcType"></param>
        /// <returns></returns>
        public static IAgent CreateAgent(string svcUrl, string httpMethod = "POST", string svcType = "WebApi")
        {
            IAgent agent = null;

            switch (svcType)
            {
                case "WebApi":
                    agent = new WebApiAgent(svcUrl, httpMethod);
                    break;
            }

            return agent;
        }
    }
}