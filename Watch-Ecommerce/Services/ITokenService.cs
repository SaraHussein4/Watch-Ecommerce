using ECommerce.Core.model;
using Microsoft.AspNetCore.Identity;

namespace Watch_Ecommerce.Services
{
    public interface ITokenService
    {
        Task<string> CreateTokenAsync(User user, UserManager<User> userManager);

    }
}
