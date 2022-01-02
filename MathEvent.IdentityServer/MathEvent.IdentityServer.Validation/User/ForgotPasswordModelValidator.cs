using MathEvent.IdentityServer.Contracts.Validation.User;
using MathEvent.IdentityServer.Models.User;
using MathEvent.IdentityServer.Models.Validation;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MathEvent.IdentityServer.Validation.User
{
    /// <summary>
    /// Валидатор модели смены пароля
    /// </summary>
    public class ForgotPasswordModelValidator : IForgotPasswordModelValidator
    {
        private readonly UserValidationUtils _userValidationUtils;

        public ForgotPasswordModelValidator(UserValidationUtils userValidationUtils)
        {
            _userValidationUtils = userValidationUtils;
        }

        public async Task<ValidationResult> Validate(ForgotPasswordModel model)
        {
            var validationErrors = new List<ValidationError>();
            validationErrors.AddRange(await _userValidationUtils.ValidateEmail(model.Email, false));

            return new ValidationResult
            {
                IsValid = validationErrors.Count < 1,
                Errors = validationErrors,
            };
        }
    }
}
