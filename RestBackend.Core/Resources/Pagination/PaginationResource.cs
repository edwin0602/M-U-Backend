using RestBackend.Core.Services;
using System;
using System.Collections.Generic;

namespace RestBackend.Core.Resources.Pagination
{
    public class PaginationResource<T>
    {
        public PaginationResource(int _PageNumber, int _PageSize)
        {
            PageNumber = _PageNumber;
            PageSize = _PageSize;
        }

        public IEnumerable<T> Data { get; set; }

        public int TotalRecords { get; set; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

    }
}
