using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Text.Json;

namespace WordCountService
{
    public class WordCountService : WordCountIService
    {
        public string WordCount(Stream file)
        {
            try
            {
                // Read the stream and convert it to a string
                StreamReader reader = new StreamReader(file);
                string fileContent = reader.ReadToEnd();

                // Trim the file content to 100_000_000 symbols
                fileContent = Trim(fileContent, 100_000_000);

                // Create a dictionary to store the word counts
                Dictionary<string, int> wordCount = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

                // Use regex to split words by non-alphabetic characters
                string[] words = System.Text.RegularExpressions.Regex.Split(fileContent, @"[^a-zA-Z]+");

                // Count occurrences of each alphabetic word
                foreach (string word in words)
                {
                    if (string.IsNullOrWhiteSpace(word)) continue;  // Skip empty entries

                    if (wordCount.ContainsKey(word))
                    {
                        wordCount[word]++;
                    }
                    else
                    {
                        wordCount[word] = 1;
                    }
                }

                // Serialize the result to JSON
                string jsonResult = JsonSerializer.Serialize(wordCount);
                return jsonResult;
            }
            catch (Exception ex)
            {
                // Handle exceptions and return an error message
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
