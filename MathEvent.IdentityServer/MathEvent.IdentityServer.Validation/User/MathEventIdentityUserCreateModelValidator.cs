using MathEvent.IdentityServer.Contracts.Validation.User;
using MathEvent.IdentityServer.Models.User;
using MathEvent.IdentityServer.Models.Validation;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MathEvent.IdentityServer.Validation.User
{
    /// <summary>
    /// Валидатор модели создания пользователя
    /// </summary>
    public class MathEventIdentityUserCreateModelValidator : IMathEventIdentityUserCreateModelValidator
    {
        private readonly UserValidationUtils _userValidationUtils;

        public MathEventIdentityUserCreateModelValidator(UserValidationUtils userValidationUtils)
        {
            _userValidationUtils = userValidationUtils;
        }

        public async Task<ValidationResult> Validate(MathEventIdentityUserCreateModel model)
        {
            var validationErrors = new List<ValidationError>();

            validationErrors.AddRange(_userValidationUtils.ValidateName(model.Name));
            validationErrors.AddRange(_userValidationUtils.ValidateSurname(model.Surname));
            validationErrors.AddRange(await _userValidationUtils.ValidateEmail(model.Email));
            validationErrors.AddRange(_userValidationUtils.ValidateUsername(model.UserName));
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
