using RestBackend.Core.Models.Business;
using RestBackend.Core.Repositories;

namespace RestBackend.Data.Repositories
{
    public class PropertiesImagesRepository : Repository<PropertyImage>, IPropertyImageRepository
    {
        public PropertiesImagesRepository(RestBackendDbContext context)
          : base(context)
        { }
    }
}
