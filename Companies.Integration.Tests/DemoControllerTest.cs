using Companies.API;
using Companies.API.DTOs;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Companies.Integration.Tests
{
    public class DemoControllerTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private HttpClient httpClient;

        public DemoControllerTest(WebApplicationFactory<Program> applicationFactory)
        {
            applicationFactory.ClientOptions.BaseAddress = new Uri("https://localhost:7273/api/demo/");
            httpClient = applicationFactory.CreateClient();
        }

        [Fact]
        public async Task ShouldReturnOk()
        {
            var response = await httpClient.GetAsync("");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task ShouldReturnExpectedMessage()
        {
            var response = await httpClient.GetAsync("");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            Assert.Equal("Working", content);
            Assert.Equal("text/plain", response.Content.Headers.ContentType.MediaType);
        }

        [Fact]
        public async Task ShouldReturnExpectedMediaType()
        {
            var response = await httpClient.GetAsync("dto");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();

            var dto = JsonSerializer.Deserialize<CompanyDto>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.Equal("Working AB", dto.Name);
            Assert.Equal("application/json", response.Content.Headers.ContentType.MediaType);

        }

        [Fact]
        public async Task Index3_ShouldReturnExpectedMessage_WithStream()
        {
            var response = await httpClient.GetStreamAsync("dto");

            var dto = await JsonSerializer.DeserializeAsync<CompanyDto>(response, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.Equal("Working AB", dto.Name);

        }

        [Fact]
        public async Task Index4_ShouldReturnExpectedMessageSimplifyed()
        {
            var dto = await httpClient.GetFromJsonAsync<CompanyDto>("dto");
            Assert.Equal("Working AB", dto.Name);

        }
    }
}
