using RestBackend.Api.Wrappers;
using RestBackend.Core.Resources.Pagination;
using RestBackend.Core.Services.Infrastructure;
using System;
using System.Linq;

namespace RestBackend.Api.Extensions
{
    public static class DataPagerExtension
    {
        public static PagedResponse<TModel> GetPaginatedResponse<TModel>(
            this PaginationResource<TModel> filter,
            IUriService uriService,
            string route
            )
        {
            var paged = new PagedResponse<TModel>();

            filter.PageSize = (filter.PageSize <= 0) ? 1 : filter.PageSize;

            paged.PageNumber = filter.PageNumber;
            paged.PageSize = filter.PageSize;

            paged.Data = filter.Data.ToList();

            paged.TotalRecords = filter.TotalRecords;
            paged.TotalPages = (int)Math.Ceiling(paged.TotalRecords / (double)paged.PageSize);

            paged.NextPage = filter.PageNumber >= 1 && filter.PageNumber < paged.TotalPages
                            ? uriService.GetPaginationUri(new PaginationFilter(filter.PageNumber + 1, filter.PageSize), route)
                            : null;

            paged.PreviousPage =
                filter.PageNumber - 1 >= 1 && filter.PageNumber <= paged.TotalPages
                ? uriService.GetPaginationUri(new PaginationFilter(filter.PageNumber - 1, filter.PageSize), route)
                : null;

            paged.FirstPage = uriService.GetPaginationUri(new PaginationFilter(1, filter.PageSize), route);
            paged.LastPage = uriService.GetPaginationUri(new PaginationFilter(paged.TotalPages, filter.PageSize), route);

            return paged;
        }
    }
}
