using System.Collections.Generic;

namespace Buytopia.Models.ViewModels
{
    public class IndexViewModel
    {
        public IEnumerable<Product> Product { get; set; }
        public IEnumerable<Category> Category { get; set; }
        public IEnumerable<Coupon> Coupon { get; set; }
    }
}
