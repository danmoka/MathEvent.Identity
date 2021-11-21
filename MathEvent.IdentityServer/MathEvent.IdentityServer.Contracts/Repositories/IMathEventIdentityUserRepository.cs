using MathEvent.IdentityServer.Entities;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace MathEvent.IdentityServer.Contracts.Repositories
{
    public interface IMathEventIdentityUserRepository : IRepositoryBase<MathEventIdentityUser>
    {
        /// <summary>
        /// Декларирует создание пользователя с паролем
        /// </summary>
        /// <param name="entity">Пользователь</param>
        /// <param name="password">Пароль</param>
        /// <returns>Результат создания</returns>
        Task<IdentityResult> CreateIdentityUser(MathEventIdentityUser entity, string password);

        /// <summary>
        /// Декларирует обновление пользователя
        /// </summary>
        /// <param name="entity">Пользователь</param>
        /// <returns>Результат обновления</returns>
        Task<IdentityResult> UpdateIdentityUser(MathEventIdentityUser entity);

        /// <summary>
        /// Декларирует удаление пользователя
        /// </summary>
        /// <param name="entity">Пользователь</param>
        /// <returns>Результат удаления</returns>
        Task<IdentityResult> DeleteIdentityUser(MathEventIdentityUser entity);
    }
}
