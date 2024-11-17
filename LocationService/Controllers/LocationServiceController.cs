using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace LocationService.Controllers
{
    public class LocationServiceController : ApiController
    {
        public HttpResponseMessage Get(string latitude, string longitude)
        {
            // Validate Latitude and Longitude
            if (!IsValidLatitude(latitude) || !IsValidLongitude(longitude))
            {
                var errorResponse = new
                {
                    Error = "Invalid latitude or longitude. Please provide correct values."
                };
                string jsonResponse = JsonConvert.SerializeObject(errorResponse);

                return new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json")
                };
            }

            // Get Detailed information about the place by coordinates
            string detailsApiUrl = $"https://nominatim.openstreetmap.org/reverse?lat={Uri.EscapeDataString(latitude)}&lon={Uri.EscapeDataString(longitude)}&format=json";

            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(GetPlaceDetails(detailsApiUrl), Encoding.UTF8, "application/json")
            };

            return response;
        }

        private string GetPlaceDetails(string apiUrl)
        {
            using (WebClient client = new WebClient())
            {
                client.Headers.Add("User-Agent", "LocationServiceExampleApp/1.0");
                
                // Fetch details
                string jsonResponse = client.DownloadString(apiUrl);
                JavaScriptSerializer js = new JavaScriptSerializer();
                var responseObj = js.Deserialize<dynamic>(jsonResponse);

                // Extract and reorganize data in a more appropriate structure
                var modifiedResponse = new
                {
                    PlaceId = GetValueIfExists(responseObj, "place_id"),
                    DisplayName = GetValueIfExists(responseObj, "display_name"),
                    Address = new
                    {
                        HouseNumber = GetValueIfExists(responseObj["address"], "house_number"),
                        Town = GetValueIfExists(responseObj["address"], "town"),
                        State = GetValueIfExists(responseObj["address"], "state"),
                        Postcode = GetValueIfExists(responseObj["address"], "postcode"),
                        Country = GetValueIfExists(responseObj["address"], "country")
                    },
                    Coordinates = new
                    {
                        Latitude = GetValueIfExists(responseObj, "lat"),
                        Longitude = GetValueIfExists(responseObj, "lon")
                    },
                    Metadata = new
                    {
                        Category = GetValueIfExists(responseObj, "class"),
                        Type = GetValueIfExists(responseObj, "type"),
                        Importance = GetValueIfExists(responseObj, "importance"),
                        PlaceRank = GetValueIfExists(responseObj, "place_rank")
                    }
                    
                };

                return js.Serialize(modifiedResponse);
            }
        }

        private object GetValueIfExists(dynamic dictionary, string key)
        {
            return dictionary.ContainsKey(key) ? dictionary[key] : null;
        }

        private bool IsValidLatitude(string latitude)
        {
            if (decimal.TryParse(latitude, out decimal lat))
            {
                return lat >= -90 && lat <= 90;
            }
            return false;
        }

        private bool IsValidLongitude(string longitude)
        {
            if (decimal.TryParse(longitude, out decimal lon))
            {
                return lon >= -180 && lon <= 180;
            }
            return false;
        }
    }
}
