using RestBackend.Core.Resources.Pagination;
using System;

namespace RestBackend.Core.Services.Infrastructure
{
    public interface IUriService
    {
        public Uri GetPaginationUri(PaginationFilter filter, string route);
    }
}
