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
            var email = context.User.Claims
                .Where(c => c.Type == ClaimTypes.Email)
                .SingleOrDefault();

            if (email is null)
            {
                context.Fail();
            }

            var user = await _mathEventIdentityUserService.GetIdentityUserByEmail(email.Value);

            if (user is null)
            {
                context.Fail();
            }

            if (requirement.Name == Operations.Read.Name
                || requirement.Name == Operations.Update.Name
                || requirement.Name == Operations.Delete.Name)
            {
                if (resource.Id == user.Id)
                {
                    context.Succeed(requirement);
                }
                else
                {
                    context.Fail();
                }
            }

            context.Succeed(requirement);
        }
    }
}
