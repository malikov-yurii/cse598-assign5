using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace ChatGPTNamespace
{
    [ServiceContract]
    public interface IChatGPTService
    {

        [OperationContract]
        Task<string> AskChatGPTAboutUrl(string question, string[] resources, string userId);

        [OperationContract]
        string getChat(string userId);

        [OperationContract]
        string AskChatGPT(string question, string userId);

        [OperationContract]
        Int16 getPromptsCountLeftToday(string userId);

        [OperationContract]
        string evaluateDevelopmentInvestmentAttractiveness(double latitude, double longitude, string userId);

    }

}
