using Microsoft.AspNetCore.Identity;
using System;

namespace MathEvent.IdentityServer.Entities
{
    /// <summary>
    /// Сущность роли
    /// </summary>
    public class MathEventIdentityRole : IdentityRole<Guid>
    {
        public MathEventIdentityRole(string role) : base(role)
        {
        }

        public MathEventIdentityRole() : base()
        {
        }
    }
}
