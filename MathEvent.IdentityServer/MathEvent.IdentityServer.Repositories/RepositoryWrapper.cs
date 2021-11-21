using MathEvent.IdentityServer.Contracts.Repositories;
using MathEvent.IdentityServer.Database;
using MathEvent.IdentityServer.Entities;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace MathEvent.IdentityServer.Repositories
{
    /// <summary>
    /// Оболочка для репозиториев. Позволяет не добавлять в DI все классы репозиториев
    /// </summary>
    public class RepositoryWrapper : IRepositoryWrapper
    {
        /// <summary>
        /// Контекст данных для работы с базой данных
        /// </summary>
        private readonly RepositoryContext _repositoryContext;

        /// <summary>
        /// Менеджер для работы с пользователями
        /// </summary>
        private readonly UserManager<MathEventIdentityUser> _userManager;

        /// <summary>
        /// Репозиторий для работы с Пользователями
        /// </summary>
        private IMathEventIdentityUserRepository _user;

        public RepositoryWrapper(RepositoryContext repositoryContext, UserManager<MathEventIdentityUser> userManager)
        {
            _repositoryContext = repositoryContext;
            _userManager = userManager;
        }

        /// <summary>
        /// Предоставляет репозиторий для работы с Пользователями
        /// </summary>
        public IMathEventIdentityUserRepository User
        {
            get
            {
                if (_user is null)
                {
                    _user = new MathEventIdentityUserRepository(_repositoryContext, _userManager);
                }

                return _user;
            }
        }

        /// <summary>
        /// Фиксирует изменения, совершенные репозиториями над контекстом данных
        /// </summary>
        /// <returns></returns>
        public async Task Save()
        {
            await _repositoryContext.SaveChangesAsync();
        }
    }
}
