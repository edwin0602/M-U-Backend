using Microsoft.Extensions.Logging;
using RestBackend.Core.Models.Notification;
using RestBackend.Core.Services.Infrastructure;
using System.Threading.Tasks;

namespace RestBackend.Infrastructure.Notification
{
    public class EmailService : IEmailService
    {
        private readonly ILogger<EmailService> _logger;

        public EmailService(
            ILogger<EmailService> logger)
        {
            _logger = logger;
        }

        public Task<bool> Send(EmailNotification notification)
        {
            _logger.LogInformation($"{notification.Sender}");
            _logger.LogInformation($"{notification.To}");
            _logger.LogInformation($"{notification.Subject}");
            _logger.LogInformation($"{notification.Message}");

            return Task.FromResult(true);
        }
    }
}
