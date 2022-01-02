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
    public class ForgotPasswordResetModelValidator : IForgotPasswordResetModelValidator
    {
        private readonly UserValidationUtils _userValidationUtils;

        public ForgotPasswordResetModelValidator(UserValidationUtils userValidationUtils)
        {
            _userValidationUtils = userValidationUtils;
        }

        public async Task<ValidationResult> Validate(ForgotPasswordResetModel model)
        {
            var validationErrors = new List<ValidationError>();

            if (string.IsNullOrEmpty(model.Token))
            {
                validationErrors.Add(new ValidationError
                {
                    Field = nameof(model.Token),
                    Message = "Токен должен быть задан"
                });
            }

            validationErrors.AddRange(await _userValidationUtils.ValidateEmail(model.Email, false));
            validationErrors.AddRange(_userValidationUtils.ValidatePassword(model.Password));

            if (model.Password != model.PasswordConfirm)
            {
                validationErrors.Add(new ValidationError
                {
                    Field = nameof(model.Password),
                    Message = "Пароли не совпадают"
                });
            }

            return new ValidationResult
            {
                IsValid = validationErrors.Count < 1,
                Errors = validationErrors,
            };
        }
    }
}
