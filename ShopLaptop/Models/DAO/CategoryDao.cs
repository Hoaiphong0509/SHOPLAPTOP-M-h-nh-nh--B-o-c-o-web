using ShopLaptop.Models.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopLaptop.Common
{
    public class CategoryDao
    {
        ShopLaptionDbContextDataContext context = null;
        
        public CategoryDao()
        {
            context = new ShopLaptionDbContextDataContext();
        }
        public List<Category> brand()
        {
            return context.Categories.ToList();
        }
    }

}