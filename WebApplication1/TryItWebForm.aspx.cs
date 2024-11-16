using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TryItWebApplication.WordCountServiceReference;
using System.Net.Http;
using System.Threading.Tasks;

namespace TryItWebApplication
{
    public partial class TryItWebForm : System.Web.UI.Page
    {
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            // Check if a file is uploaded
            if (fileUpload.HasFile)
            {
                try
                {
                    // Convert the uploaded file to a Stream
                    Stream fileStream = fileUpload.FileContent;

                    // Limit the file size to 100_000_000 bytes
                    if (fileStream.Length > 100_000_000)
                    {
                        txtCountWordResult.Text = "Cannot process file. File size exceeds 100_000_000 bytes.";
                        return;
                    }

                    // Invoke the WordCount WCF service using the Stream
                    WordCountServiceReference.WordCountIServiceClient client = new WordCountServiceReference.WordCountIServiceClient();

                    // The WordCount method accepts Stream
                    string wordCountResult = client.WordCount(fileStream);
                    client.Close();

                    // Display the result on the page
                    txtCountWordResult.Text = wordCountResult;
                }
                catch (Exception ex)
                {
                    txtCountWordResult.Text = "Error: " + ex.Message;
                }
            }
            else
            {
                txtCountWordResult.Text = "Please upload a valid text file.";
            }
        }

        protected async void btnDownload_Click(object sender, EventArgs e)
        {
            string url = txtUrl.Text.Trim();
            url = Trim(url, 1000);
            txtWebDownloadResult.Text = await GetResourseContent(url);
        }

        private async Task<string> GetResourseContent(string url)
        {
            var result = "";
            if (!string.IsNullOrEmpty(url))
            {
                try
                {
                    // Base address of your WebDownloading REST service
                    string serviceBaseUrl = "http://localhost:62930/WebDownloadingService.svc/";

                    // Build the full URL to call the WebDownload method with the given URL parameter
                    string requestUrl = $"{serviceBaseUrl}/WebDownload?url={Uri.EscapeDataString(url)}";

                    using (HttpClient client = new HttpClient())
                    {
                        // Send a GET request to the WebDownload REST service
                        HttpResponseMessage response = await client.GetAsync(requestUrl);

                        // Check if the request was successful
                        if (response.IsSuccessStatusCode)
                        {
                            // Read the content of the response
                            result = await response.Content.ReadAsStringAsync();
                        }
                        else
                        {
                            // Handle unsuccessful response
                            result = $"Error: {response.StatusCode}";
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Handle any exceptions that occur during the request
                    result = $"Exception: {ex.Message}";
                }
            }
            else
            {
                result = "Please enter a valid URL.";
            }

            return result;
        }

        protected async void btnAskChatGPT_Click(object sender, EventArgs e)
        {
            string prompt = txtPrompt.Text.Trim();
            string url1 = txtUrl1.Text.Trim();
            string url2 = txtUrl2.Text.Trim();

            // Trim inputs
            prompt = Trim(prompt, 1000);
            url1 = Trim(url1, 100);
            url2 = Trim(url2, 100);

            // Prepare the array of URLs
            string[] urls = new string[] { url1, url2 };

            if (!string.IsNullOrEmpty(prompt))
            {
                try
                {
                    // Create a client to call the ChatGPT service
                    ChatGPTServiceReference.ChatGPTServiceClient client = new ChatGPTServiceReference.ChatGPTServiceClient();

                    // Call the AskChatGPTAboutUrl service method asynchronously with hardcoded chatId
                    string chatGPTResponse = await Task.Run(() => client.AskChatGPTAboutUrl(prompt, urls, "test-chat-id"));
                    client.Close();

                    // Display the result in the TextBox
                    txtGptResult.Text = chatGPTResponse;
                }
                catch (Exception ex)
                {
                    txtGptResult.Text = $"Error: {ex.Message}";
                }
            }
            else
            {
                txtGptResult.Text = "Please enter a valid prompt. It is empty now";
            }
        }

        protected async void btnGetChatHistory_Click(object sender, EventArgs e)
        {
            string chatId = txtChatId.Text.Trim();

            chatId = Trim(chatId, 100);

            if (!string.IsNullOrEmpty(chatId))
            {
                try
                {
                    // Create a client to call the ChatGPT service
                    ChatGPTServiceReference.ChatGPTServiceClient client = new ChatGPTServiceReference.ChatGPTServiceClient();

                    // Call the getChat service method
                    string chatHistory = await Task.Run(() => client.getChat(chatId));
                    client.Close();

                    // Display the chat history in the text box
                    txtChatHistory.Text = chatHistory;
                }
                catch (Exception ex)
                {
                    txtChatHistory.Text = $"Error: {ex.Message}";
                }
            }
            else
            {
                txtChatHistory.Text = "Please enter a valid chat ID.";
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
