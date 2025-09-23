using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Core.Application.Common.Filter
{
    public class PaginationFilter
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        //public int TotalRecord { get; set; }
        public PaginationFilter()
        {
            this.PageNumber = 1;
            this.PageSize = 60;
        }
        public PaginationFilter(int pageNumber, int pageSize)
        {
            this.PageNumber = pageNumber < 1 ? 1 : pageNumber;
            this.PageSize = pageSize > 60 ? 60 : pageSize;
        }
    }
}
