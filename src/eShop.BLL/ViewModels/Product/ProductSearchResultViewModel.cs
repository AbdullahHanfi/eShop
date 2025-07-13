using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShop.BLL.ViewModels.Product
{
    public class ProductSearchResultViewModel
    {
        public List<ProductViewModel> Products { get; set; }
        public string SearchTerm { get; set; }
        public int TotalItems { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public string SortBy { get; set; }
        public string SortOrder { get; set; }
        public int PageSize { get; set; } = 12;

        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;
    }
}
