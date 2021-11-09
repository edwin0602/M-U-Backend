using RestBackend.Core.Models.Business;
using RestBackend.Core.Repositories;

namespace RestBackend.Data.Repositories
{
    public class PropertyRepository : Repository<Property>, IPropertyRepository
    {
        public PropertyRepository(RestBackendDbContext context)
          : base(context)
        { }
    }
}
