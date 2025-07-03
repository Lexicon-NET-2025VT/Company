using Companies.Shared.DTOs;
using Microsoft.AspNetCore.Identity;

namespace Services.Contracts
{
    public interface IAuthService
    {
        Task<IdentityResult> RegisterUserAsync(UserForRegistrationDto registrationDto);
        Task<bool> ValidateUserAsync(UserForAuthDto userForAuthDto);
    }
}