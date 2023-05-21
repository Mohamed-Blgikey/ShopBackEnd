using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specification.ProductSpec
{
    public class ProductSpecParams
    {
        private const int MaxSize = 50;
        public int PageIndex { set; get; } = 1;
        private int pageSize = 6;

        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = (value > MaxSize) ? MaxSize : value ; }
        }

        public int? BrandId { get; set; }
        public int? TypeId { get; set; }
        public string? Sort { get; set; }

        private string _search;

        public string? Search
        {
            get { return _search; }
            set { _search = value.ToLower(); }
        }

    }
}
