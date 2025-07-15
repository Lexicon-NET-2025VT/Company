using AutoMapper;
using Companies.Infrastructure.Data;
using Companies.Presentation.Controllers.ControllersForTestDemo;
using Companies.Shared.DTOs;
using Controller.Tests.Extensions;
using Domain.Contracts;
using Domain.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client.Extensions.Msal;
using Moq;
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

        public RepoControllerTest()
        {
            mockRepo = new Mock<IEmployeeRepository>();

            var mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfile>();
            }));

            var mockUserStore = new Mock<IUserStore<ApplicationUser>>();
            userManager = new Mock<UserManager<ApplicationUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            sut = new RepositoryController(mockRepo.Object, mapper, userManager.Object);
        }

        [Fact]
        public async Task GetEmployees_ShouldReturnAllEmployees()
        {
            var users = GetUsers();
            mockRepo.Setup(x => x.GetEmployeesAsync(It.IsAny<int>(), It.IsAny<bool>())).ReturnsAsync(users);

            // sut.SetUserIsAuth(true);

            userManager.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(new ApplicationUser { UserName = "Kalle" });

            var result = await sut.GetEmployees(1);

            // Asserta
            var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
            var items = Assert.IsType<List<EmployeeDto>>(okObjectResult.Value);
            Assert.Equal(items.Count, users.Count);

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
