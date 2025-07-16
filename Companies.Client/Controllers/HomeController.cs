using Companies.API.DTOs;
using Companies.Client.Clients;
using Companies.Client.Models;
using Companies.Shared.DTOs;
using Microsoft.AspNetCore.JsonPatch;
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
        private readonly ICompaniesClient companiesClient;

        public HomeController(IHttpClientFactory httpClientFactory, ICompaniesClient companiesClient)
        {

            // httpClient = new HttpClient();
            //httpClient = httpClientFactory.CreateClient();

            //httpClient.BaseAddress = new Uri("https://localhost:7273");

            httpClient = httpClientFactory.CreateClient("CompaniesClient");
            this.companiesClient = companiesClient;
        }

        public async Task<IActionResult> Index()
        {
            var result = await SimpleGetAsync();
            var result2 = await SimpleGetAsync2();

            var result3 = await GetWithRequestMessageAsync();

            var result4 = await PostWithRequestMessageAsync();

            await PatchWithRequestMessageAsync();

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
            //var request = new HttpRequestMessage(HttpMethod.Get, "api/companies");
            //request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(json));
            //var response = await httpClient.SendAsync(request);
            //response.EnsureSuccessStatusCode();
            //var res = await response.Content.ReadAsStringAsync();
            //var companies = JsonSerializer.Deserialize<IEnumerable<CompanyDto>>(res, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            //return companies!;

            return await companiesClient.GetAsync<IEnumerable<CompanyDto>>("api/companies");
        }

        private async Task<CompanyDto> PostWithRequestMessageAsync()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "api/companies");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(json));
            var companyToCreate = new CompanyCreateDto
            {
                Name = "AB Skrot och korn",
                Address = "Storgatan 5",
                Country = "Sweden",
                Employees = null
            };
            var jsonCompany = JsonSerializer.Serialize(companyToCreate);
            request.Content = new StringContent(jsonCompany);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue(json);
            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var res = await response.Content.ReadAsStringAsync();
            var companyDto = JsonSerializer.Deserialize<CompanyDto>(res, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            var location = response.Headers.Location;


            return companyDto;
        }

        private async Task PatchWithRequestMessageAsync()
        {
            var patchDocument = new JsonPatchDocument<EmployeeUpdateDto>();
            patchDocument.Replace(e => e.Age, 40);
            patchDocument.Replace(e => e.Name, "Kalle Anka");

            var serializedPatchDoc = Newtonsoft.Json.JsonConvert.SerializeObject(patchDocument);

            var request = new HttpRequestMessage(HttpMethod.Patch, "api/companies/1/employees/040e02ad-cdbb-4826-907b-d6b793f68cf8");

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(json));

            request.Content = new StringContent(serializedPatchDoc);

            request.Content.Headers.ContentType = new MediaTypeHeaderValue(json);

            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();



        }
    }
}
