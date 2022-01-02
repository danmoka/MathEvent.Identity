using MathEvent.IdentityServer.Contracts.Validation.User;
using MathEvent.IdentityServer.Models.User;
using MathEvent.IdentityServer.Models.Validation;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MathEvent.IdentityServer.Validation.User
{
    /// <summary>
    /// Валидатор модели обновления пользователя
    /// </summary>
    public class MathEventIdentityUserUpdateModelValidator : IMathEventIdentityUserUpdateModelValidator
    {
        private readonly UserValidationUtils _userValidationUtils;

        public MathEventIdentityUserUpdateModelValidator(UserValidationUtils userValidationUtils)
        {
            _userValidationUtils = userValidationUtils;
        }

        public async Task<ValidationResult> Validate(MathEventIdentityUserUpdateModel model)
        {
            var validationErrors = new List<ValidationError>();

            validationErrors.AddRange(_userValidationUtils.ValidateName(model.Name));
            validationErrors.AddRange(_userValidationUtils.ValidateSurname(model.Surname));

            return new ValidationResult
            {
                IsValid = validationErrors.Count < 1,
                Errors = validationErrors,
            };
        }
    }
}
