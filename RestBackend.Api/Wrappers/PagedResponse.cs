using System;
using System.Collections.Generic;

namespace RestBackend.Api.Wrappers
{
    public class PagedResponse<T> : ResposeBase
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public Uri FirstPage { get; set; }
        public Uri LastPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalRecords { get; set; }
        public Uri NextPage { get; set; }
        public Uri PreviousPage { get; set; }

        public List<T> Data { get; set; }

        public PagedResponse() : base()
        {
            this.Data = new List<T>();
        }

    }
}
