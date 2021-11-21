using MimeKit;
using System.Collections.Generic;
using System.Linq;

namespace MathEvent.IdentityServer.Models.Email
{
    /// <summary>
    /// Модель сообщения
    /// </summary>
    public class Message
    {
        public List<MailboxAddress> To { get; set; }

        public string Subject { get; set; }

        public string Content { get; set; }

        /// <summary>
        /// Создает экземпляр сообщения 
        /// </summary>
        /// <param name="to">Набор адресатов</param>
        /// <param name="subject">Тема</param>
        /// <param name="content">Сообщение</param>
        public Message(IEnumerable<string> to, string subject, string content)
        {
            To = new List<MailboxAddress>();
            To.AddRange(to.Select(x => new MailboxAddress(x)));
            Subject = subject;
            Content = content;
        }
    }
}
