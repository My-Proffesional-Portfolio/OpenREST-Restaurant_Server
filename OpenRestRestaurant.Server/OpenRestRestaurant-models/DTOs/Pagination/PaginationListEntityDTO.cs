using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenRestRestaurant_models.DTOs.Pagination
{
    public class PaginationListEntityDTO<T>
    {
        public PaginationListEntityDTO()
        {
            PagedList = new List<T>();
        }
        public List<T> PagedList { get; set; }
        public long PageNumber { get; set; }
        public long TotalCount { get; set; }
        public decimal TotalPages { get; set; }
    }
}
