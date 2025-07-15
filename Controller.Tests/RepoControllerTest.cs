using AutoMapper;
using Companies.Infrastructure.Data;
using Companies.Presentation.Controllers.ControllersForTestDemo;
using Companies.Shared.DTOs;
using Controller.Tests.Extensions;
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
    public class RepoControllerTest
    {
        private Mock<IEmployeeRepository> mockRepo;
        private Mock<UserManager<ApplicationUser>> userManager;
        private RepositoryController sut;
        private Mock<IUnitOfWork> mockUow;
        private Mock<IServiceManager> serviceManagerMock;
        private Mapper mapper;

        public RepoControllerTest()
        {
            // mockUow = new Mock<IUnitOfWork>();  
            serviceManagerMock = new Mock<IServiceManager>();

            mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfile>();
            }));

            var mockUserStore = new Mock<IUserStore<ApplicationUser>>();
            userManager = new Mock<UserManager<ApplicationUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            sut = new RepositoryController(serviceManagerMock.Object, mapper, userManager.Object);
        }

        [Fact]
        public async Task GetEmployees_ShouldReturnAllEmployees()
        {
            var users = GetUsers();

            var dtos = mapper.Map<IEnumerable<EmployeeDto>>(users);
            ApiBaseResponse baseResponse = new ApiOkResponse<IEnumerable<EmployeeDto>>(dtos);

            // mockUow.Setup(x=> x.EmployeeRepository.GetEmployeesAsync(It.IsAny<int>(), It.IsAny<bool>())).ReturnsAsync(users);
            serviceManagerMock.Setup(x => x.EmployeeService.GetEmployeesAsync(It.IsAny<int>())).ReturnsAsync(baseResponse);


            userManager.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(new ApplicationUser { UserName = "Kalle" });

            var result = await sut.GetEmployees(1);

            // Asserta
            var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
            var items = Assert.IsType<List<EmployeeDto>>(okObjectResult.Value);
            Assert.Equal(items.Count, users.Count);

        }


        [Fact]
        public async Task GetEmployees_ShouldThrowExeptionIfUserNotFound()
        {
            await Assert.ThrowsAsync<ArgumentException>(async () => await sut.GetEmployees(1));
        }



        public List<ApplicationUser> GetUsers()
        {
            return new List<ApplicationUser>
            {
                new ApplicationUser
                {
                     Id = "1",
                     Name = "Kalle",
                     Age = 12,
                     UserName = "Kalle"
                },
               new ApplicationUser
                {
                     Id = "2",
                     Name = "Kalle",
                     Age = 12,
                     UserName = "Kalle"
                },
            };

        }
    }
}
