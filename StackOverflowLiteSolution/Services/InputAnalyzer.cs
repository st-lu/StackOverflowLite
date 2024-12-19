using Stackoverflow_Lite.models;
using Stackoverflow_Lite.Services.Interfaces;

namespace Stackoverflow_Lite.Services
{
    public class InputAnalyzer : IInputAnalyzer
    {
        HttpClient _httpClient;
        private string _baseUrl;

        public InputAnalyzer(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _baseUrl = configuration["InputAnalyzer:BASE_URL"];
        }

        public async Task<TextCategory> Analyze(string text, CancellationToken cancellation)
        {
            var result = await _httpClient.PostToEndpointAsync<PredictionResponse>(_baseUrl + "/predict", new { text });
            return (TextCategory)result!.Prediction;
        }
    }
}
