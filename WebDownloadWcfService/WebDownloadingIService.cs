using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace ChatGPTNamespace
{
    [ServiceContract]
    public interface WebDownloadingIService
    {
        [OperationContract]
        [WebGet(UriTemplate = "/WebDownload?url={url}", ResponseFormat = WebMessageFormat.Json)]
        string WebDownload(string url);
    }

}