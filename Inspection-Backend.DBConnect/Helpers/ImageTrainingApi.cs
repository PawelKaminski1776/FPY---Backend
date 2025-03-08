using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using InspectionBackend.Contracts.InspectionDtos;
using System.Net.Http.Headers;

namespace MongoDB.Helpers
{
    public class ImageTrainingAPI
    {
        private readonly HttpClient _client = new HttpClient();
        private readonly string Url;
        private readonly string _username;
        private readonly string _password;

        public ImageTrainingAPI(string url, string username, string password)
        {
            Url = url;
            _username = username;
            _password = password;
        }

        public async Task<InspectionImageResponse?> SendToImageTrainingAPI(string endpoint, object message)
        {
            try
            {
                string jsonMessage = JsonSerializer.Serialize(message);
                HttpContent content = new StringContent(jsonMessage, Encoding.UTF8, "application/json");

                var byteArray = Encoding.ASCII.GetBytes($"{_username}:{_password}");
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));


                using HttpResponseMessage response = await _client.PostAsync(Url + endpoint, content);

  
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<InspectionImageResponse>(responseBody);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine($"Message: {e.Message}");
                return null;
            }
            catch (JsonException e)
            {
                Console.WriteLine("\nJSON Deserialization Error!");
                Console.WriteLine($"Message: {e.Message}");
                return null;
            }
        }
    }
}
