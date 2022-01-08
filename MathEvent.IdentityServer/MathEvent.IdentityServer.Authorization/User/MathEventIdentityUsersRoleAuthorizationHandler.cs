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
    public class MathEventIdentityUsersRoleAuthorizationHandler :
        AuthorizationHandler<OperationAuthorizationRequirement, MathEventIdentityUserRoleModel>
    {
        private readonly IMathEventIdentityUserService _mathEventIdentityUserService;

        public MathEventIdentityUsersRoleAuthorizationHandler(IMathEventIdentityUserService userService)
        {
            _mathEventIdentityUserService = userService;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            OperationAuthorizationRequirement requirement,
            MathEventIdentityUserRoleModel resource)
        {
            if (requirement.Name == Operations.Create.Name
                || requirement.Name == Operations.Delete.Name)
            {
                var email = context.User.Claims
                    .Where(c => c.Type == ClaimTypes.Email)
                    .SingleOrDefault();

                if (email is not null)
                {
                    var user = await _mathEventIdentityUserService.GetIdentityUserByEmail(email.Value);

                    if (user is not null && await _mathEventIdentityUserService.IsInRole(user.Id, MathEventIdentityServerRoles.Administrator))
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
