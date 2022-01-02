namespace MathEvent.IdentityServer.Models.User
{
    /// <summary>
    /// Модель передачи данных при смене пароля
    /// </summary>
    public class ForgotPasswordModel
    {
        public string Email { get; set; }
    }
}
