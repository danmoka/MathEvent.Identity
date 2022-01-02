using IdentityServer4.Models;
using IdentityServer4.Services;
using MathEvent.IdentityServer.Contracts.Services;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MathEvent.IdentityServer
{
    /// <summary>
    /// Настройка профиля пользователя
    /// </summary>
    public class ProfileService : IProfileService
    {
        protected IMathEventIdentityUserService _mathEventIdentityUserService;

        public ProfileService(IMathEventIdentityUserService mathEventIdentityUserService)
        {
            _mathEventIdentityUserService = mathEventIdentityUserService;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var user = await _mathEventIdentityUserService.GetIdentityUser(context.Subject);
            var roles = await _mathEventIdentityUserService.GetIdentityUserRoles(user);

            var claims = new List<Claim>
            {
                new Claim("sub", user.Id.ToString()),
                new Claim("email", user.Email),
                new Claim("email_verified", user.EmailConfirmed.ToString()),
                new Claim("roles", JsonConvert.SerializeObject(roles)),
                new Claim("name", user.UserName),
                new Claim("given_name", $"{user.Name} {user.Surname}" ),
                //new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                //new Claim(ClaimTypes.Email, user.Email),
                //new Claim(ClaimTypes.Role, JsonConvert.SerializeObject(roles)),
                //new Claim(ClaimTypes.Name, user.UserName),
                //new Claim(ClaimTypes.GivenName, $"{user.Name} {user.Surname}" ),
            };

            context.IssuedClaims.AddRange(claims);
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var user = await _mathEventIdentityUserService.GetIdentityUser(context.Subject);

            context.IsActive = user is not null;
        }
    }
}
