using MathEvent.IdentityServer.Models.Email;

namespace MathEvent.IdentityServer.Contracts.Services
{
    /// <summary>
    /// Декларирует функциональность отправителя email сообщений
    /// </summary>
    public interface IEmailService
    {
        void SendEmail(Message message);
    }
}
