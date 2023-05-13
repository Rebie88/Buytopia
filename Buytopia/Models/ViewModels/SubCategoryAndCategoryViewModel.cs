using System.Collections.Generic;

namespace Buytopia.Models.ViewModels
{
    public class SubCategoryAndCategoryViewModel
    {
        public IEnumerable<Category> CategoryList { get; set; }
        public SubCategory SubCategory { get; set; }
        //I chosed string list b/c we need only the name of subcategory
        public List<string> SubCategoryList { get; set; }
        //the string alart if anything went wrong to show error message
        public string StatusMessage { get; set; }
    }
}
