using AutoMapper;
using Companies.Infrastructure.Data;
using Companies.Presentation.Controllers.ControllersForTestDemo;
using Domain.Contracts;
using Microsoft.Identity.Client.Extensions.Msal;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller.Tests
{
    internal class RepoControllerTest
    {
        private Mock<IEmployeeRepository> mockRepo;
        private RepositoryController sut;

        public RepoControllerTest()
        {
            mockRepo = new Mock<IEmployeeRepository>();

            var mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfile>();
            }));

            sut = new RepositoryController(mockRepo.Object, mapper);
        }
    }
}
