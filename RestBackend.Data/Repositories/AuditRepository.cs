using RestBackend.Core.Models.Security;
using RestBackend.Core.Repositories;

namespace RestBackend.Data.Repositories
{
    public class AuditRepository : Repository<Audit>, IAuditRepository
    {
        public AuditRepository(RestBackendDbContext context)
           : base(context)
        { }
    }
}
