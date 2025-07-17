using Companies.API;
using Companies.API.DTOs;
using Companies.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Companies.Integration.Tests
{
    public class DemoControllerWithCustomFactoryTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private HttpClient httpClient;
        private CompaniesContext context;

        public DemoControllerWithCustomFactoryTests(CustomWebApplicationFactory<Program> applicationFactory)
        {
            applicationFactory.ClientOptions.BaseAddress = new Uri("https://localhost:7273/api/demo/");
            httpClient = applicationFactory.CreateClient();
            context = applicationFactory.Context;
        }

        [Fact]
        public async Task Get_ShouldReturnCompany_FromInMemoryDatabase()
        {
            var dto = await httpClient.GetFromJsonAsync<CompanyDto>("getone");
            Assert.Equal("TestCompanyName", dto.Name);
        }

        [Fact]
        public async Task GetAll_ShouldReturnAllCompanies()
        {
            var dtos = await httpClient.GetFromJsonAsync<IEnumerable<CompanyDto>>("getall");
            Assert.Equal(context.Companies.Count(), dtos.Count());
              
        }
    }
}
