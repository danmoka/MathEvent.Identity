using MathEvent.IdentityServer.Models.User;

namespace MathEvent.IdentityServer.Contracts.Validation.User
{
    /// <summary>
    /// Декларирует функциональность валидатора модели смены пароля
    /// </summary>
    public interface IForgotPasswordModelValidator : IValidator<ForgotPasswordModel>
    {
    }
}
