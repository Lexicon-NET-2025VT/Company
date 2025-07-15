using Companies.API.DTOs;
using Companies.Presentation.Controllers;
using Controller.Tests.Extensions;
using Controller.Tests.TestFixtures;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Utilities;
using Moq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Controller.Tests
{
    public class SimpleControllerTests : IClassFixture<DatabasFixture>
    {
        private readonly DatabasFixture fixture;

        public SimpleControllerTests(DatabasFixture fixture)
        {
            this.fixture = fixture;
        }


        [Fact]
        public async Task GetCompany_ShouldReturnExpectedCount()
        {
            var sut = fixture.Sut;
            var expectedCount = fixture.Context.Companies.Count();

            var result = await sut.GetCompany2();

            var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
            var items = Assert.IsType<List<CompanyDto>>(okObjectResult.Value);

            Assert.Equal(expectedCount, items.Count);
        }

        [Fact]
        public async Task GetCompany_Should_Return400()
        {
            var sut = fixture.Sut;
            var res = await sut.GetCompany();
            var resultType = res.Result as BadRequestObjectResult;
            Assert.IsType<BadRequestObjectResult>(resultType);
            Assert.Equal(StatusCodes.Status400BadRequest, resultType.StatusCode);
        }

        [Fact]
        public async Task GetCompany_IfNotAuth_ShouldReturn400BadRequest()
        {
            var httpContextMock = new Mock<HttpContext>();

            httpContextMock.SetupGet(x => x.User.Identity.IsAuthenticated).Returns(false);

            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContextMock.Object
            };

            var sut = fixture.Sut;
            sut.ControllerContext = controllerContext;

            var res = await sut.GetCompany();

            var resultType = res.Result as BadRequestObjectResult;

            Assert.IsType<BadRequestObjectResult>(resultType);
            Assert.Equal("Is not auth", resultType.Value);
        }

        [Fact]
        public async Task GetCompany_IfNotAuth_ShouldReturn400BadRequest2()
        {
            //var mockClaimsPrincipal = new Mock<ClaimsPrincipal>();
            //mockClaimsPrincipal.SetupGet(x => x.Identity.IsAuthenticated).Returns(false);

            var sut = fixture.Sut;
            sut.SetUserIsAuth(false);



            //sut.ControllerContext = new ControllerContext
            //{
            //    HttpContext = new DefaultHttpContext()
            //    {
            //        User = mockClaimsPrincipal.Object
            //    }
            //};

            var result = await sut.GetCompany();
            var resultType = result.Result as BadRequestObjectResult;

            Assert.IsType<BadRequestObjectResult>(resultType);
            Assert.Equal(StatusCodes.Status400BadRequest, resultType.StatusCode);
        }

        [Fact]
        public async Task GetCompany_IsAuth_ShouldReturn200()
        {
            var sut = fixture.Sut;
            sut.SetUserIsAuth(true);

            var result = await sut.GetCompany();
            var resultType = result.Result as OkObjectResult;

            Assert.IsType<OkObjectResult>(resultType);
        }
    }
}