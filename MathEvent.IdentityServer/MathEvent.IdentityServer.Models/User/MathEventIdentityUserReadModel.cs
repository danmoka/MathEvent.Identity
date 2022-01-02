using System;

namespace MathEvent.IdentityServer.Models.User
{
    /// <summary>
    /// Класс для передачи данных для чтения информации о пользователе
    /// </summary>
    public class MathEventIdentityUserReadModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }
    }
}
