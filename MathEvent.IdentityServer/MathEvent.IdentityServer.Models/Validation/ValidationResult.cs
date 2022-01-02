using System.Collections.Generic;

namespace MathEvent.IdentityServer.Models.Validation
{
    /// <summary>
    /// Описывает результат валидации
    /// </summary>
    public class ValidationResult
    {
        public bool IsValid { get; set; }

        public IEnumerable<ValidationError> Errors { get; set; }
    }
}
