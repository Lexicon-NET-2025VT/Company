using AutoMapper;
using Companies.Infrastructure.Data;
using Companies.Presentation.Controllers.ControllersForTestDemo;
using Companies.Shared.DTOs;
using Controller.Tests.Extensions;
using Controller.Tests.TestFixtures;
using Domain.Contracts;
using Domain.Models.Entities;
using Domain.Models.Responses;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client.Extensions.Msal;
using Moq;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Controller.Tests
{
    public class RepoControllerTest : IClassFixture<RepoControllerFixture>
    {
        private readonly RepoControllerFixture fixture;


        public RepoControllerTest(RepoControllerFixture fixture)
        {
            this.fixture = fixture;



            //serviceManagerMock = new Mock<IServiceManager>();

            //mapper = new Mapper(new MapperConfiguration(cfg =>
            //{
            //    cfg.AddProfile<AutoMapperProfile>();
            //}));

            //var mockUserStore = new Mock<IUserStore<ApplicationUser>>();
            //userManager = new Mock<UserManager<ApplicationUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            //sut = new RepositoryController(serviceManagerMock.Object, mapper, userManager.Object);
        }

        [Fact]
        public async Task GetEmployees_ShouldReturnAllEmployees()
        {
            var users = fixture.GetUsers();

            var dtos = fixture.Mapper.Map<IEnumerable<EmployeeDto>>(users);
            ApiBaseResponse baseResponse = new ApiOkResponse<IEnumerable<EmployeeDto>>(dtos);

            // mockUow.Setup(x=> x.EmployeeRepository.GetEmployeesAsync(It.IsAny<int>(), It.IsAny<bool>())).ReturnsAsync(users);
            fixture.ServiceManagerMock.Setup(x => x.EmployeeService.GetEmployeesAsync(It.IsAny<int>())).ReturnsAsync(baseResponse);


            fixture.UserManager.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(new ApplicationUser { UserName = "Kalle" });

            var result = await fixture.Sut.GetEmployees(1);

            // Asserta
            var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
            var items = Assert.IsType<List<EmployeeDto>>(okObjectResult.Value);
            Assert.Equal(items.Count, users.Count);

        }


        [Fact]
        public async Task GetEmployees_ShouldThrowExeptionIfUserNotFound()
        {
            await Assert.ThrowsAsync<ArgumentException>(async () => await fixture.Sut.GetEmployees(1));
        }



        //public List<ApplicationUser> GetUsers()
        //{
        //    return new List<ApplicationUser>
        //    {
        //        new ApplicationUser
        //        {
        //             Id = "1",
        //             Name = "Kalle",
        //             Age = 12,
        //             UserName = "Kalle"
        //        },
        //       new ApplicationUser
        //        {
        //             Id = "2",
        //             Name = "Kalle",
        //             Age = 12,
        //             UserName = "Kalle"
        //        },
        //    };

        //}
    }
}
