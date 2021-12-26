using MathEvent.IdentityServer.Constants;
using MathEvent.IdentityServer.Contracts.Services;
using MathEvent.IdentityServer.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MathEvent.IdentityServer.Authorization.User
{
    /// <summary>
    /// Обработчик запроса на авторизацию для CRUD операций пользователей
    /// </summary>
    public class MathEventIdentityUsersAuthorizationCrudHandler :
        AuthorizationHandler<OperationAuthorizationRequirement, MathEventIdentityUserReadModel>
    {
        private readonly IMathEventIdentityUserService _mathEventIdentityUserService;

        public MathEventIdentityUsersAuthorizationCrudHandler(IMathEventIdentityUserService userService)
        {
            _mathEventIdentityUserService = userService;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            OperationAuthorizationRequirement requirement,
            MathEventIdentityUserReadModel resource)
        {
            if (requirement.Name == Operations.Read.Name
                || requirement.Name == Operations.Update.Name
                || requirement.Name == Operations.Delete.Name)
            {
                var email = context.User.Claims
                    .Where(c => c.Type == ClaimTypes.Email)
                    .SingleOrDefault();

                if (email is not null)
                {
                    var user = await _mathEventIdentityUserService.GetIdentityUserByEmail(email.Value);

                    if (user is not null && resource.Id == user.Id)
                    {
                        context.Succeed(requirement);
                        return;
                    }

                    if (user is not null && await _mathEventIdentityUserService.IsInRole(user, MathEventIdentityServerRoles.Administrator))
                    {
                        context.Succeed(requirement);
                        return;
                    }
                }
            }

            context.Fail();
        }
    }
}
