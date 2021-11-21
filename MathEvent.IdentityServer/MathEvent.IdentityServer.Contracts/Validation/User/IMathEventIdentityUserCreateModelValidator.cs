using MathEvent.IdentityServer.Models.User;

namespace MathEvent.IdentityServer.Contracts.Validation.User
{
    /// <summary>
    /// Декларирует функциональность валидатора модели создания пользователя
    /// </summary>
    public interface IMathEventIdentityUserCreateModelValidator : IValidator<MathEventIdentityUserCreateModel>
    {
    }
}
