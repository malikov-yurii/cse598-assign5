using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WordCountService
{
    [ServiceContract]
    public interface WordCountIService
    {
        [OperationContract]
        string WordCount(Stream file);

    }
}
