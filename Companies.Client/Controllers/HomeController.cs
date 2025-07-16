using Companies.API.DTOs;
using Companies.Client.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace Companies.Client.Controllers
{
    public class HomeController : Controller
    {
        private HttpClient httpClient;
        private const string json = "application/json";

        public HomeController()
        {
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("https://localhost:7273");
        }

        public async Task<IActionResult> Index()
        {
            var result = await SimpleGetAsync();
            var result2 = await SimpleGetAsync2();

            var result3 = await GetWithRequestMessageAsync();

            return View();
        }

        private async Task<IEnumerable<CompanyDto>> SimpleGetAsync()
        {
            var response = await httpClient.GetAsync("api/companies");
            response.EnsureSuccessStatusCode();
            var res = await response.Content.ReadAsStringAsync();

            var companies  = JsonSerializer.Deserialize<IEnumerable<CompanyDto>>(res, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase});

            return companies!;
            
        }

        private async Task<IEnumerable<CompanyDto>> SimpleGetAsync2()
        {
            return await httpClient.GetFromJsonAsync<IEnumerable<CompanyDto>>("api/companies", new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        }

        private async Task<IEnumerable<CompanyDto>> GetWithRequestMessageAsync()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "api/companies");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(json));
            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var res = await response.Content.ReadAsStringAsync();
            var companies = JsonSerializer.Deserialize<IEnumerable<CompanyDto>>(res, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            return companies!;
        }
    }
}
