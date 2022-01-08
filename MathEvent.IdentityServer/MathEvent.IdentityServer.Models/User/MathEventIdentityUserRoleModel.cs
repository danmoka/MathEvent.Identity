using System;
using System.ComponentModel.DataAnnotations;

namespace MathEvent.IdentityServer.Models.User
{
    public class MathEventIdentityUserRoleModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Role { get; set; }
    }
}
