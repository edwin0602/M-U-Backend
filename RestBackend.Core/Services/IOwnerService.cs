using RestBackend.Core.Resources;
using RestBackend.Core.Resources.Pagination;
using System.Threading.Tasks;

namespace RestBackend.Core.Services
{
    public interface IOwnerService
    {
        Task<OwnerResource> Create(CreateOwnerResource toCreate);

        Task<PaginationResource<OwnerResource>> GetAll(PaginationFilter pagination);

        Task<PaginationResource<PropertyResource>> GetPropertiesByOwner(PaginationFilter pagination, int IdOwner);

        Task<OwnerResource> GetById(int id);

        Task Update(int id, OwnerResource owner);

    }
}
