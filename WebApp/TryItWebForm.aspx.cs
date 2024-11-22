﻿using System;
using System.Threading.Tasks;
using System.Web.UI;
using WebApplication1.ChatGPTServiceReference;

namespace TryItWebApplication
{
    public partial class TryItWebForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                UpdatePromptsLeftForUser();
            }
        }

        protected void LoginControl_LoginStatusChanged(object sender, EventArgs e)
        {
            UpdatePromptsLeftForUser();
            // Clear any outputs
            txtGptResult.Text = "";
            txtChatHistory.Text = "";
        }

        private void UpdatePromptsLeftForUser()
        {
            try
            {
                if (Session["userId"] != null)
                {
                    string userId = Session["userId"].ToString();
                    // Create a client to call the ChatGPT service
                    using (ChatGPTServiceClient client = new ChatGPTServiceClient())
                    {
                        // Fetch the remaining prompts for the user
                        Int16 promptsLeft = client.getPromptsCountLeftToday(userId);

                        // Display the result in the label
                        lblPromptsLeft.Text = $"You have {promptsLeft} prompts left for today.";
                    }
                }
                else
                {
                    lblPromptsLeft.Text = "Please log in.";
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions and display error message
                lblPromptsLeft.Text = $"Error fetching prompts: {ex.Message}";
            }
        }

        protected async void btnAskChatGPT_Click(object sender, EventArgs e)
        {
            if (Session["userId"] == null)
            {
                txtGptResult.Text = "You must be logged in to ask ChatGPT.";
                return;
            }

            string userId = Session["userId"].ToString();
            string latitudeStr = txtLatitude.Text.Trim();
            string longitudeStr = txtLongitude.Text.Trim();

            // Validate inputs
            if (double.TryParse(latitudeStr, out double latitude) &&
                double.TryParse(longitudeStr, out double longitude) &&
                IsValidLatitude(latitude) &&
                IsValidLongitude(longitude))
            {
                try
                {
                    // Create a client to call the ChatGPT service
                    using (ChatGPTServiceClient client = new ChatGPTServiceClient())
                    {
                        string chatGPTResponse = await Task.Run(() => client.evaluateDevelopmentInvestmentAttractiveness(latitude, longitude, userId));
                        // Display the result in the TextBox
                        txtGptResult.Text = chatGPTResponse;
                        UpdatePromptsLeftForUser();
                    }
                }
                catch (Exception ex)
                {
                    txtGptResult.Text = $"Error: {ex.Message}";
                }
            }
            else
            {
                txtGptResult.Text = "Please enter valid latitude (-90 to 90) and longitude (-180 to 180).";
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
            if (Session["userId"] == null)
            {
                txtChatHistory.Text = "You must be logged in to view chat history.";
                return;
            }

            string userId = Session["userId"].ToString();

            try
            {
                // Create a client to call the ChatGPT service
                using (ChatGPTServiceClient client = new ChatGPTServiceClient())
                {
                    // Call the getChat service method
                    string chatHistory = await Task.Run(() => client.getChat(userId));
                    // Display the chat history in the text box
                    txtChatHistory.Text = chatHistory;
                }
            }
            catch (Exception ex)
            {
                txtChatHistory.Text = $"Error: {ex.Message}";
            }
        }
    }
}
