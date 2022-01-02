namespace MathEvent.IdentityServer.Models.User
{
    /// <summary>
    /// Модель передачи данных при смене пароля
    /// </summary>
    public class ForgotPasswordResetModel
    {
        public string Token { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string PasswordConfirm { get; set; }
    }
}
