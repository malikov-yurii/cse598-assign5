using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace ChatGPTNamespace
{
    public class ChatGPTService : IChatGPTService
    {
        // Using System.Diagnostics for logging
        private static readonly TraceSource traceSource = new TraceSource("ChatGPTWcfService");

        // You can use this key or your own ChatGPT key if the existing one is already disabled.
        private static readonly string openAIAPIKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
        private static readonly string openAIAPIEndpoint = "https://api.openai.com/v1/chat/completions";
        private static readonly string ChatFileName = Path.Combine(AppDomain.CurrentDomain.GetData("DataDirectory").ToString(), "gptChats.json");

        private const int MAX_PROMPTS_PER_DAY = 10;

        // Constructor
        public ChatGPTService()
        {
            // Log API key initialization
            traceSource.TraceEvent(TraceEventType.Information, 0, $"API Key loaded: {openAIAPIKey?.Substring(0, 15)}...");
        }

        public async Task<string> AskChatGPTAboutUrl(string question, string[] contextResources, string userId)
        {
            if (string.IsNullOrEmpty(question))
            {
                return "Empty question";
            }

            if (string.IsNullOrEmpty(userId))
            {
                return "Missing userId";
            }

            question = Trim(question, 1000);

            // Initialize a StringBuilder for thread-safe string concatenation
            StringBuilder questionContext = new StringBuilder(" FYI Question context: ");
            object lockObject = new object(); // Separate lock object for thread safety

            List<Task> tasks = new List<Task>();

            // Process each URL asynchronously
            foreach (string url in contextResources)
            {
                if (string.IsNullOrEmpty(url))
                {
                    continue;
                }
                tasks.Add(Task.Run(async () =>
                {
                    string urlContent = await GetResourceContent(url);
                    urlContent = Trim(urlContent, 8000);

                    string context = "&& URL: [[" + url + "]] Content: [[" + urlContent + "]]";

                    traceSource.TraceEvent(TraceEventType.Verbose, 0, $"GetResourceContent Response: {context}");

                    // Lock the shared resource to make the operation thread-safe
                    lock (lockObject)
                    {
                        questionContext.Append(context);
                    }
                }));
            }

            // Wait for all tasks to complete
            await Task.WhenAll(tasks);

            var fullQuestion = question + questionContext.ToString();
            // Trim the fullQuestion to 20000 symbols
            fullQuestion = Trim(fullQuestion, 20000);

            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"fullQuestion: {fullQuestion}");

            // Ask ChatGPT asynchronously
            var gptResponse = await AskChatGPTAsync(fullQuestion, userId);

            // Persist chat with question and response
            await PersistChatAsync(userId, question, gptResponse, contextResources);

            return gptResponse;
        }

        private async Task PersistChatAsync(string userId, string question, string gptResponse, string[] resources)
        {
            // Load existing chat history or create a new chat structure
            var chatHistory = LoadChatHistory();

            // Get the current time (epoch)
            long epochTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            // Prepare the chat entry for the question
            var userEntry = new ChatEntry
            {
                Actor = "USER",
                Text = question,
                Resources = resources,
                Time = epochTime
            };

            // Prepare the chat entry for the response
            var gptEntry = new ChatEntry
            {
                Actor = "ChatGPT",
                Text = gptResponse,
                Resources = resources,
                Time = epochTime
            };

            // Append or create the chat for the given userId
            if (!chatHistory.Chats.ContainsKey(userId))
            {
                chatHistory.Chats[userId] = new Chat
                {
                    ChatEntries = new List<ChatEntry>()
                };
            }

            // Add both the user's question and the GPT response to the chat
            chatHistory.Chats[userId].ChatEntries.AddRange(new[] { userEntry, gptEntry });

            // Save the updated chat history back to the file
            SaveChatHistory(chatHistory);
        }

        private ChatHistory LoadChatHistory()
        {
            try
            {
                // Ensure the App_Data folder exists
                string directory = Path.GetDirectoryName(ChatFileName);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                // If the file doesn't exist, create an empty chat history file
                if (!File.Exists(ChatFileName))
                {
                    var emptyHistory = new ChatHistory();
                    SaveChatHistory(emptyHistory);  // Create the file with empty history
                    return emptyHistory;
                }

                // Load the chat history from the file
                string json = File.ReadAllText(ChatFileName);
                return JsonSerializer.Deserialize<ChatHistory>(json) ?? new ChatHistory();
            }
            catch (UnauthorizedAccessException ex)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, $"Access denied to file {ChatFileName}: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, $"Error loading chat history: {ex.Message}");
                throw;
            }
        }

        private void SaveChatHistory(ChatHistory chatHistory)
        {
            try
            {
                // Ensure the App_Data folder exists
                string directory = Path.GetDirectoryName(ChatFileName);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                // Save the chat history to the file
                string json = JsonSerializer.Serialize(chatHistory, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(ChatFileName, json);
            }
            catch (UnauthorizedAccessException ex)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, $"Access denied to file {ChatFileName}: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, $"Error saving chat history: {ex.Message}");
                throw;
            }
        }

        public string AskChatGPT(string question, string userId)
        {
            try
            {
                // Trim the question to 20000 symbols
                question = Trim(question, 20000);

                // Call the async method and get the result
                var result = AskChatGPTAsync(question, userId).GetAwaiter().GetResult();
                return result;
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

        private async Task<string> AskChatGPTAsync(string question, string userId)
        {
            if (string.IsNullOrEmpty(question))
            {
                return "";
            }

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {openAIAPIKey}");

                // Request body for OpenAI API (ChatGPT model)
                var requestBody = new
                {
                    model = "gpt-3.5-turbo", // Specifying the ChatGPT model
                    messages = new[]
                    {
                        new { role = "system", content = "You are ChatGPT, a helpful assistant." },
                        new { role = "user", content = question }
                    }
                };

                // Serialize the request body using System.Text.Json
                string jsonRequestBody = JsonSerializer.Serialize(requestBody);

                // Send a POST request
                var response = await client.PostAsync(openAIAPIEndpoint, new StringContent(jsonRequestBody, Encoding.UTF8, "application/json"));

                // Ensure the response is successful
                response.EnsureSuccessStatusCode();

                // Get the response body as a string
                string responseBody = await response.Content.ReadAsStringAsync();

                // Deserialize the response using System.Text.Json
                var chatGPTResponse = JsonSerializer.Deserialize<OpenAIResponse>(responseBody);

                // Extract the response text
                var gptResponse = chatGPTResponse.choices[0].message.content;

                // Persist the chat
                await PersistChatAsync(userId, question, gptResponse, null);

                return gptResponse;
            }
        }

        private async Task<string> GetResourceContent(string url)
        {
            var result = "";
            if (!string.IsNullOrEmpty(url))
            {
                try
                {
                    // Base address of WebDownloading REST service
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

                            // Remove HTML tags and JavaScript from the result
                            result = StripHtmlTags(result);
                        }
                        else
                        {
                            // Log the error response
                            traceSource.TraceEvent(TraceEventType.Error, 0, $"Failed to download from URL: {url}. Status Code: {response.StatusCode}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Log the exception
                    traceSource.TraceEvent(TraceEventType.Error, 0, $"Exception occurred: {ex.Message}\n{ex.StackTrace}");
                }
            }
            else
            {
                // Log that no URL was provided
                traceSource.TraceEvent(TraceEventType.Warning, 0, "No valid URL was provided.");
            }

            return result;
        }

        public string getChat(string userId)
        {
            // Load existing chat history
            var chatHistory = LoadChatHistory();

            // Check if the userId exists in the loaded chat history
            if (chatHistory.Chats.ContainsKey(userId))
            {
                // Get the chat for the given userId
                var chat = chatHistory.Chats[userId];

                // Reverses the list in-place to see latest on top
                chat.ChatEntries.Reverse();

                // Serialize the chat to JSON
                string jsonChat = JsonSerializer.Serialize(chat, new JsonSerializerOptions { WriteIndented = true });

                return jsonChat;
            }
            else
            {
                // If the userId does not exist, return an error message in JSON format
                return JsonSerializer.Serialize(new { error = "Chat not found", userId = userId });
            }
        }

        public Int16 getPromptsCountLeftToday(string userId)
        {
            int promptsLeft = MAX_PROMPTS_PER_DAY;
            try
            {
                var chatHistory = LoadChatHistory();

                if (chatHistory.Chats.ContainsKey(userId))
                {
                    var chatEntries = chatHistory.Chats[userId].ChatEntries;

                    // Get current date in UTC
                    DateTime today = DateTime.UtcNow.Date;

                    // Count the number of prompts made by the user today
                    int promptsToday = chatEntries.Count(entry =>
                        entry.Actor == "USER" &&
                        DateTimeOffset.FromUnixTimeSeconds(entry.Time).UtcDateTime.Date == today);

                    promptsLeft = MAX_PROMPTS_PER_DAY - promptsToday;
                    if (promptsLeft < 0)
                        promptsLeft = 0;
                }
            }
            catch (Exception ex)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, $"Error in getPromptsCountLeftToday: {ex.Message}");
                // In case of error, assume no prompts left
                promptsLeft = 0;
            }
            return (Int16)promptsLeft;
        }

        public string evaluateDevelopmentInvestmentAttractiveness(double latitude, double longitude, string userId)
        {
            try
            {
                // Compose a question for ChatGPT
                string question = $"Evaluate the development investment attractiveness " +
                    $"of the location at latitude {latitude} and longitude {longitude}. " +
                    $"Provide an analysis including economic, environmental, and social factors for " +
                    $"building new estate and installing solar panels. Speculate in 100 words. " +
                    $"Start with nearest city name and estate and solar development investment Attractiveness level where top is 10 and low is 0";

                // Call AskChatGPT with a system userId or a special identifier
                string response = AskChatGPT(question, userId);

                return response;
            }
            catch (Exception ex)
            {
                traceSource.TraceEvent(TraceEventType.Error, 0, $"Error in evaluateDevelopmentInvestmentAttractiveness: {ex.Message}");
                return $"Error: {ex.Message}";
            }
        }

        // Method to trim input strings to limit symbols
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

        // Simple method to strip HTML tags and JavaScript from a string
        private string StripHtmlTags(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            // Remove <script>...</script> blocks (including multiline content and any attributes)
            input = Regex.Replace(input, @"<script[^>]*>.*?<\/script>", string.Empty, RegexOptions.Singleline | RegexOptions.IgnoreCase);

            // Remove <style>...</style> blocks (including multiline content and any attributes)
            input = Regex.Replace(input, @"<style[^>]*>.*?<\/style>", string.Empty, RegexOptions.Singleline | RegexOptions.IgnoreCase);

            // Remove HTML tags
            input = Regex.Replace(input, @"<[^>]+>", string.Empty, RegexOptions.IgnoreCase);

            input = Regex.Replace(input, @"\\n", " ", RegexOptions.IgnoreCase);
            input = Regex.Replace(input, @"\\t", " ", RegexOptions.IgnoreCase);
            input = Regex.Replace(input, @" +", " ", RegexOptions.IgnoreCase);

            // Optionally, decode HTML entities like &quot; to their respective characters
            input = System.Net.WebUtility.HtmlDecode(input);

            return input.Trim(); // Return the cleaned text
        }

        // Classes to match OpenAI response structure
        public class OpenAIResponse
        {
            public Choice[] choices { get; set; }
        }

        public class Choice
        {
            public Message message { get; set; }
        }

        public class Message
        {
            public string role { get; set; }
            public string content { get; set; }
        }

        // Chat structure for persistence
        public class ChatHistory
        {
            public Dictionary<string, Chat> Chats { get; set; } = new Dictionary<string, Chat>();
        }

        public class Chat
        {
            public List<ChatEntry> ChatEntries { get; set; } = new List<ChatEntry>();
        }

        public class ChatEntry
        {
            public string Actor { get; set; }
            public string Text { get; set; }
            public string[] Resources { get; set; }
            public long Time { get; set; }
        }
    }
}
