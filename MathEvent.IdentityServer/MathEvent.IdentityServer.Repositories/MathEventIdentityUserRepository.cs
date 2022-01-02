using MathEvent.IdentityServer.Contracts.Repositories;
using MathEvent.IdentityServer.Database;
using MathEvent.IdentityServer.Entities;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace MathEvent.IdentityServer.Repositories
{
    /// <summary>
    /// Репозиторий для Пользователей
    /// </summary>
    public class MathEventIdentityUserRepository : RepositoryBase<MathEventIdentityUser>, IMathEventIdentityUserRepository
    {
        private readonly UserManager<MathEventIdentityUser> _userManager;

        public MathEventIdentityUserRepository(
            RepositoryContext repositoryContext,
            UserManager<MathEventIdentityUser> userManager) : base(repositoryContext)
        {
            _userManager = userManager;
        }

        /// <summary>
        /// Создает пользователя с паролем
        /// </summary>
        /// <param name="entity">Пользователь</param>
        /// <param name="password">Пароль</param>
        /// <returns>Результат создания пользователя</returns>
        public async Task<IdentityResult> CreateIdentityUser(MathEventIdentityUser entity, string password)
        {
            return await _userManager.CreateAsync(entity, password);
        }

        /// <summary>
        /// Обновляет пользователя
        /// </summary>
        /// <param name="entity">Пользователь</param>
        /// <returns>Результат обновления</returns>
        public async Task<IdentityResult> UpdateIdentityUser(MathEventIdentityUser entity)
        {
            return await _userManager.UpdateAsync(entity);
        }

        /// <summary>
        /// Удаляет пользователя
        /// </summary>
        /// <param name="entity">Пользователь</param>
        /// <returns>Результат удаления</returns>
        public async Task<IdentityResult> DeleteIdentityUser(MathEventIdentityUser entity)
        {
            return await _userManager.DeleteAsync(entity);
        }
    }
}
