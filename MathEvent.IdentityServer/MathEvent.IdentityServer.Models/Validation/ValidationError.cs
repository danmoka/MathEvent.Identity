namespace MathEvent.IdentityServer.Models.Validation
{
    /// <summary>
    /// Описывает ошибку валидации
    /// </summary>
    public class ValidationError
    {
        public string Field { get; set; }

        public string Message { get; set; }
    }
}
