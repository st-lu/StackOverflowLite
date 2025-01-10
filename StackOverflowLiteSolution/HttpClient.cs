using System.Text;
using System.Text.Json;

namespace Stackoverflow_Lite
{
    public class HttpClient
    {
        private readonly IHttpClientFactory _clientFactory;

        public HttpClient(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<T?> PostToEndpointAsync<T>(string url, object data)
        {
            var client = _clientFactory.CreateClient();
            
            
            var content = new StringContent(
                JsonSerializer.Serialize(data),
                Encoding.UTF8,
                "application/json");
            
            Console.WriteLine("Content is:");
            Console.WriteLine(await content.ReadAsStringAsync());

            var response = await client.PostAsync(url, content);
            Console.WriteLine("Response is:");
            Console.WriteLine(response);
            

            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<T>(responseBody, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
    }
}
