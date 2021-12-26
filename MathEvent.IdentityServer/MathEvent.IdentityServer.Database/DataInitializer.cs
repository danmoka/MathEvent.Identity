using MathEvent.IdentityServer.Constants;
using MathEvent.IdentityServer.Database.Configuration;
using MathEvent.IdentityServer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MathEvent.IdentityServer.Database
{
    /// <summary>
    /// Заполняет базу данных
    /// </summary>
    public static class DataInitializer
    {
        public static async Task Initialize(
            RoleManager<MathEventIdentityRole> roleManager,
            UserManager<MathEventIdentityUser> userManager,
            IConfiguration configuration)
        {
            await InitializeRoles(roleManager);

            //Only on initial launch
            await InitializeAdministratorAccount(userManager, configuration);
        }

        private static async Task InitializeRoles(RoleManager<MathEventIdentityRole> roleManager)
        {
            var applicationRoles = new List<string>
            {
                MathEventIdentityServerRoles.Administrator,
                MathEventIdentityServerRoles.User,
                MathEventRoles.Administrator,
                MathEventRoles.Moderator,
            };

            foreach (var role in applicationRoles)
            {
                if (await roleManager.FindByNameAsync(role) == null)
                    await roleManager.CreateAsync(new MathEventIdentityRole(role));
            }
        }

        private static async Task InitializeAdministratorAccount(UserManager<MathEventIdentityUser> userManager, IConfiguration configuration)
        {
            var adminAccounts = configuration.GetSection("Admins").Get<AdminAccountModel[]>();

            foreach (var adminAccount in adminAccounts)
            {
                if (await userManager.FindByEmailAsync(adminAccount.Email) == null)
                {
                    var administrator = new MathEventIdentityUser
                    {
                        Email = adminAccount.Email,
                        UserName = adminAccount.UserName,
                        Name = adminAccount.Name,
                        Surname = adminAccount.Surname,
                    };
                    var result = await userManager.CreateAsync(administrator, adminAccount.Password);

                    if (result.Succeeded)
                        await userManager.AddToRolesAsync(administrator, new List<string>()
                    {
                        MathEventIdentityServerRoles.Administrator,
                        MathEventIdentityServerRoles.User,
                        MathEventRoles.Administrator,
                    });
                }
            }
        }
    }
}
