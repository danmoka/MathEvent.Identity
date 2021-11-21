using MathEvent.IdentityServer.Entities;
using MathEvent.IdentityServer.Models.User;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MathEvent.IdentityServer.Contracts.Services
{
    public interface IMathEventIdentityUserService
    {
        Task<MathEventIdentityUserReadModel> Retrieve(Guid id);

        Task<MathEventIdentityUserReadModel> Create(MathEventIdentityUserCreateModel createModel);

        Task<MathEventIdentityUserReadModel> Update(Guid id, MathEventIdentityUserUpdateModel updateModel);

        Task Delete(Guid id);

        Task<MathEventIdentityUser> GetIdentityUser(ClaimsPrincipal userPrincipal);

        Task<MathEventIdentityUser> GetIdentityUserByEmail(string email);

        Task<IList<string>> GetIdentityUserRoles(MathEventIdentityUser user);

        Task ForgotPassword(string email);

        Task ResetPassword(ForgotPasswordResetModel resetModel);
    }
}
