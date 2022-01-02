using System.Threading.Tasks;

namespace MathEvent.IdentityServer.Contracts.Repositories
{
    public interface IRepositoryWrapper
    {
        IMathEventIdentityUserRepository User { get; }

        Task Save();
    }
}
