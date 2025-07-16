using Companies.API.DTOs;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Companies.Client.Clients
{
    public class CompaniesClient : ICompaniesClient
    {
        private readonly HttpClient client;
        private const string json = "application/json";

        public CompaniesClient(HttpClient client)
        {
            this.client = client;
            client.BaseAddress = new Uri("https://localhost:7273");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(json));
            client.Timeout = new TimeSpan(0, 0, 5);
        }

        public async Task<T> GetAsync<T>(string path, string contentType = json)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, path);

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

            var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();
            var stream = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<T>(stream, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            return result;
        }
    }
}
