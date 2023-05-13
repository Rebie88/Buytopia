using System.Collections.Generic;

namespace Buytopia.Models.ViewModels
{
    public class ProductItemViewModel
    {
        public Product Product { get; set; }
        public IEnumerable<Category> Category { get; set; }
        public IEnumerable<SubCategory> SubCategory { get; set; }
    }
}
