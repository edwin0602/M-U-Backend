using RestBackend.Core.Models.Security;
using System.Threading.Tasks;

namespace RestBackend.Core.Services.Infrastructure
{
    public interface IAuditService
    {
        Task Add(Audit entry);

        Task Add(string action, string target, object entity);
    }
}
