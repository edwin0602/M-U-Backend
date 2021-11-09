using RestBackend.Core.Models.Business;
using RestBackend.Core.Repositories;

namespace RestBackend.Data.Repositories
{
    public class OwnerRepository : Repository<Owner>, IOwnerRepository
    {
        public OwnerRepository(RestBackendDbContext context)
           : base(context)
        { }
    }
}
