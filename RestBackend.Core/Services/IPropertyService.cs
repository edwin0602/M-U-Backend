using Microsoft.AspNetCore.Http;
using RestBackend.Core.Resources;
using RestBackend.Core.Resources.Pagination;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestBackend.Core.Services
{
    public interface IPropertyService
    {
        Task<PropertyResource> Create(CreatePropertyResource toCreate);

        Task<PaginationResource<PropertyResource>> GetAll(PaginationFilter pagination, PropertyFilterResource filter);

        Task<PropertyResource> GetById(int id);

        Task Update(int id, PropertyResource property);

        Task SellProperty(int idProperty, decimal price, int buyerUserId);

        Task UpdatePrice(int idProperty, decimal price);

        Task AddImage(int idProperty, IFormFile image);

        Task RemoveImage(int idProperty, int idImage);

        Task<IEnumerable<PropertyImageResource>> GetImages(int idProperty);

        Task<IEnumerable<PropertyTraceResource>> GetTrace(int idProperty);
    }
}
