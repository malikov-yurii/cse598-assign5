using Newtonsoft.Json;
using System;
using System.Net;
using System.Web.UI;

namespace WebApplication
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnGetLocationData_Click(object sender, EventArgs e)
        {
            string latitude = txtLatitude.Text;
            string longitude = txtLongitude.Text;

            if (!string.IsNullOrEmpty(latitude) && !string.IsNullOrEmpty(longitude))
            {
                string locationData = GetLocationData(latitude, longitude);
                DisplayLocationData(locationData);
            }
            else
            {
                lblLocationData.Text = "Please enter both latitude and longitude.";
            }
        }

        private string GetLocationData(string latitude, string longitude)
        {
            // TODO replace localhost with real link
            string apiUrl = $"http://webstrar102.fulton.asu.edu/page8/api/locationservice?latitude={latitude}&longitude={longitude}";

            using (WebClient client = new WebClient())
            {
                // Make a request to the API and get the JSON result
                string responseString = client.DownloadString(apiUrl);
                return responseString;
            }
        }

        private void DisplayLocationData(string jsonData)
        {
            try
            {
                var locationData = JsonConvert.DeserializeObject<dynamic>(jsonData);
                if (locationData != null)
                {
                    string htmlOutput = "<table style='width:100%; border-collapse:collapse; font-family:Arial, sans-serif;'>";
                    htmlOutput += "<thead style='background-color:#f2f2f2;'><tr><th style='padding:10px; border-bottom:1px solid #ddd;'>Property</th><th style='padding:10px; border-bottom:1px solid #ddd;'>Value</th></tr></thead><tbody>";
                    htmlOutput += "<tr><td style='padding:10px; border-bottom:1px solid #ddd;'>PlaceId</td><td style='padding:10px; border-bottom:1px solid #ddd;'>" + locationData.PlaceId + "</td></tr>";
                    htmlOutput += "<tr><td style='padding:10px; border-bottom:1px solid #ddd;'>DisplayName</td><td style='padding:10px; border-bottom:1px solid #ddd;'>" + locationData.DisplayName + "</td></tr>";

                    // Address details
                    htmlOutput += "<tr><td colspan='2' style='padding:10px; border-bottom:1px solid #ddd; background-color:#f9f9f9;'><strong>Address</strong></td></tr>";
                    foreach (var addressKey in locationData.Address)
                    {
                        htmlOutput += "<tr><td style='padding:10px; border-bottom:1px solid #ddd;'>" + addressKey.Name + "</td><td style='padding:10px; border-bottom:1px solid #ddd;'>" + (addressKey.Value != null ? addressKey.Value.ToString() : "N/A") + "</td></tr>";
                    }

                    // Coordinates
                    htmlOutput += "<tr><td colspan='2' style='padding:10px; border-bottom:1px solid #ddd; background-color:#f9f9f9;'><strong>Coordinates</strong></td></tr>";
                    htmlOutput += "<tr><td style='padding:10px; border-bottom:1px solid #ddd;'>Latitude</td><td style='padding:10px; border-bottom:1px solid #ddd;'>" + locationData.Coordinates.Latitude + "</td></tr>";
                    htmlOutput += "<tr><td style='padding:10px; border-bottom:1px solid #ddd;'>Longitude</td><td style='padding:10px; border-bottom:1px solid #ddd;'>" + locationData.Coordinates.Longitude + "</td></tr>";

                    // Metadata 
                    htmlOutput += "<tr><td colspan='2' style='padding:10px; border-bottom:1px solid #ddd; background-color:#f9f9f9;'><strong>Metadata</strong></td></tr>";
                    htmlOutput += "<tr><td style='padding:10px; border-bottom:1px solid #ddd;'>Category</td><td style='padding:10px; border-bottom:1px solid #ddd;'>" + locationData.Metadata.Category + "</td></tr>";
                    htmlOutput += "<tr><td style='padding:10px; border-bottom:1px solid #ddd;'>Type</td><td style='padding:10px; border-bottom:1px solid #ddd;'>" + locationData.Metadata.Type + "</td></tr>";
                    htmlOutput += "<tr><td style='padding:10px; border-bottom:1px solid #ddd;'>Importance</td><td style='padding:10px; border-bottom:1px solid #ddd;'>" + locationData.Metadata.Importance + "</td></tr>";

                    // PlaceRank as horizontal bar chart
                    int placeRank = locationData.Metadata.PlaceRank;
                    htmlOutput += "<tr><td style='padding:10px; border-bottom:1px solid #ddd;'>PlaceRank</td><td style='padding:10px; border-bottom:1px solid #ddd;'><div style='background-color:lightblue; width:" + ((placeRank / 30.0) * 100) + "%; height:20px;'></div>" + placeRank + "</td></tr>";

                    htmlOutput += "</tbody></table>";
                    lblLocationData.Text = htmlOutput;
                }
                else
                {
                    lblLocationData.Text = "Invalid data received.";
                }
            }
            catch (Exception ex)
            {
                lblLocationData.Text = "Error parsing data: " + ex.Message;
            }
        }
    }
}