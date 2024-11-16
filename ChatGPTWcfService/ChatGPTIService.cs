using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace WordCountService
{
    [ServiceContract]
    public interface IChatGPTService
    {

        [OperationContract]
        Task<string> AskChatGPTAboutUrl(string question, string[] resources, string chatId);

        [OperationContract]
        string getChat(string chatId);

        [OperationContract]
        string AskChatGPT(string question);

    }

}
