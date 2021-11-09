using RestBackend.Core.Models.Business;
using RestBackend.Core.Repositories;

namespace RestBackend.Data.Repositories
{
    public class TaxRepository : Repository<Tax>, ITaxRepository
    {
        public TaxRepository(RestBackendDbContext context)
          : base(context)
        { }
    }
}
