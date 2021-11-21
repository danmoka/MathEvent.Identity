using MathEvent.IdentityServer.Models.Validation;
using System.Threading.Tasks;

namespace MathEvent.IdentityServer.Contracts.Validation
{
    /// <summary>
    /// Декларирует функциональность валидатора
    /// </summary>
    /// <typeparam name="T">Тип объекта валидации</typeparam>
    public interface IValidator<T> where T : class
    {
        Task<ValidationResult> Validate(T obj);
    }
}
