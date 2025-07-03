using AutoMapper;
using Companies.Shared.DTOs;
using Domain.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Companies.Services
{
    public class AuthService : IAuthService
    {
        private readonly IMapper mapper;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private ApplicationUser? user;

        public AuthService(IMapper mapper, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.mapper = mapper;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }
        public async Task<IdentityResult> RegisterUserAsync(UserForRegistrationDto registrationDto)
        {
            ArgumentNullException.ThrowIfNull(registrationDto);

            var roleExists = await roleManager.RoleExistsAsync(registrationDto.Role);
            if (!roleExists)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Role does not exist" });
            }

            var user = mapper.Map<ApplicationUser>(registrationDto);

            var result = await userManager.CreateAsync(user, registrationDto.Password!);

            if(result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, registrationDto.Role);
            }

            return result;
        }

        public async Task<bool> ValidateUserAsync(UserForAuthDto userForAuthDto)
        {
            if (userForAuthDto is null)
            {
                throw new ArgumentNullException(nameof(userForAuthDto));
            }

            user = await userManager.FindByNameAsync(userForAuthDto.UserName);
            return user != null && await userManager.CheckPasswordAsync(user, userForAuthDto.PassWord);
        }
    }
}
