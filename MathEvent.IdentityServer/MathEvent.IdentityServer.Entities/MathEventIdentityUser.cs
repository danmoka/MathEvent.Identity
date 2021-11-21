using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace MathEvent.IdentityServer.Entities
{
    /// <summary>
    /// Сущность пользователя
    /// </summary>
    public class MathEventIdentityUser : IdentityUser<Guid>
    {
        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string Name { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string Surname { get; set; }
    }
}
