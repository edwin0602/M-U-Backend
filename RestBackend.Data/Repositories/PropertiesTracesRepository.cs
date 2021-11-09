using RestBackend.Core.Models.Business;
using RestBackend.Core.Repositories;

namespace RestBackend.Data.Repositories
{
    public class PropertiesTracesRepository : Repository<PropertyTrace>, IPropertyTraceRepository
    {
        public PropertiesTracesRepository(RestBackendDbContext context)
          : base(context)
        { }
    }
}
