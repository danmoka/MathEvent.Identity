using MathEvent.IdentityServer.Contracts.Validation.User;
using MathEvent.IdentityServer.Models.User;
using MathEvent.IdentityServer.Models.Validation;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MathEvent.IdentityServer.Validation.User
{
    public class MathEventIdentityUserRoleModelValidator : IMathEventIdentityUserRoleModelValidator
    {
        private readonly UserValidationUtils _userValidationUtils;

        public MathEventIdentityUserRoleModelValidator(UserValidationUtils userValidationUtils)
        {
            _userValidationUtils = userValidationUtils;
        }

        public async Task<ValidationResult> Validate(MathEventIdentityUserRoleModel model)
        {
            var validationErrors = new List<ValidationError>();

            validationErrors.AddRange(await _userValidationUtils.ValidateUserId(model.Id));

            return new ValidationResult
            {
                IsValid = validationErrors.Count < 1,
                Errors = validationErrors,
            };
        }

    }
}
