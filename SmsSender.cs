using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Patrient_Record
{
    internal class SmsSender
    {
        private static readonly HttpClient client = new HttpClient();

        public async Task<bool> SendReminderSmsAsync(string toPhoneNumber, string message)
        {
            var apiKey = "afd701ed"; // Replace with your API key
            var apiSecret = "xDIO3nJuzJKIxoQs"; // Replace with your API secret
            var from = "639566481052"; // Replace with your sender ID or phone number

            var url = "https://rest.nexmo.com/sms/json";
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("afd701ed", apiKey),
                new KeyValuePair<string, string>("xDIO3nJuzJKIxoQs", apiSecret),
                new KeyValuePair<string, string>("639566481052", from),
                new KeyValuePair<string, string>("ContactNumber", toPhoneNumber),
                new KeyValuePair<string, string>("HOY MAY GUMANA KA NA!!!", message)
            });

            try
            {
                var response = await client.PostAsync(url, content);
                var responseString = await response.Content.ReadAsStringAsync();

                // Output the response (for debugging purposes)
                Console.WriteLine(responseString);

                // Check if the response indicates success
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred: {ex.Message}");
                return false;
            }
        }
    }
}
