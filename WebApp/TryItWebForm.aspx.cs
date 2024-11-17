using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Net.Http;
using System.Threading.Tasks;

namespace TryItWebApplication
{
    public partial class TryItWebForm : System.Web.UI.Page
    {
        protected async void btnAskChatGPT_Click(object sender, EventArgs e)
        {
            string userId = txtUserId.Text.Trim();
            string latitudeStr = txtLatitude.Text.Trim();
            string longitudeStr = txtLongitude.Text.Trim();

            // Validate inputs
            if (!string.IsNullOrEmpty(userId) &&
                double.TryParse(latitudeStr, out double latitude) &&
                double.TryParse(longitudeStr, out double longitude) &&
                IsValidLatitude(latitude) &&
                IsValidLongitude(longitude))
            {
                try
                {
                    // Create a client to call the ChatGPT service
                    WebApplication1.ChatGPTServiceReference.ChatGPTServiceClient client = new WebApplication1.ChatGPTServiceReference.ChatGPTServiceClient();

                    // Call the AskChatGPTAboutUrl service method asynchronously with hardcoded chatId
                    string chatGPTResponse = await Task.Run(() => client.evaluateDevelopmentInvestmentAttractiveness(latitude, longitude, userId));
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
                txtGptResult.Text = "Please enter a valid userId (testuser), latitude (-90 to 90), and longitude (-180 to 180).";
            }
        }

        // Helper methods for validating latitude and longitude
        private bool IsValidLatitude(double latitude)
        {
            return latitude >= -90 && latitude <= 90;
        }

        private bool IsValidLongitude(double longitude)
        {
            return longitude >= -180 && longitude <= 180;
        }


        protected async void btnGetChatHistory_Click(object sender, EventArgs e)
        {
            string userId = txtUserId.Text.Trim();

            userId = Trim(userId, 100);

            if (!string.IsNullOrEmpty(userId))
            {
                try
                {
                    // Create a client to call the ChatGPT service
                    WebApplication1.ChatGPTServiceReference.ChatGPTServiceClient client = new WebApplication1.ChatGPTServiceReference.ChatGPTServiceClient();

                    // Call the getChat service method
                    string chatHistory = await Task.Run(() => client.getChat(userId));
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
                txtChatHistory.Text = "Please enter a valid User ID. Try use: testuser";
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
