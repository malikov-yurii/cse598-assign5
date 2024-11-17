using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Text.Json;

namespace ChatGPTNamespace
{
    public class WebDownloadingService : WebDownloadingIService
    {
        public string WebDownload(string url)
        {
            try
            {
                // Trim the URL to 500 symbols
                url = Trim(url, 500);

                // Create a WebClient channel
                WebClient channel = new WebClient();

                // Download the content as a string
                string content = channel.DownloadString(url);

                // Return the content
                return content;
            }
            catch (Exception ex)
            {
                // Handle exceptions such as invalid URLs or connectivity issues
                return $"Error: {ex.Message}";
            }
        }

        // Method to trim input strings to a specified limit
        private string Trim(string input, int limit)
        {
            if (!string.IsNullOrEmpty(input))
            {
                input = input.Trim();
                if (input.Length > limit)
                {
                    input = input.Substring(0, limit);
                }
            }
            return input;
        }

    }

}
