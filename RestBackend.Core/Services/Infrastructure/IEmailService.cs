using RestBackend.Core.Models.Notification;
using System.Threading.Tasks;

namespace RestBackend.Core.Services.Infrastructure
{
    public interface IEmailService
    {
        Task<bool> Send(EmailNotification notification);
    }
}
