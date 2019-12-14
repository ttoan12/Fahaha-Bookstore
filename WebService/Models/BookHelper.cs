using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebService.Models
{
    public static class BookHelper
    {
        public static List<Book> Search(List<Book> products, SearchModel model)
        {
            if (model.Id != null)
            {
                products = products.Where(x => x.ID == model.Id).ToList();
            }
            if (!string.IsNullOrEmpty(model.Name))
            {
                products = products.Where(x => x.Name.ToLower().Contains(model.Name.ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(model.BookType))
            {
                products = products.Where(x => x.BookType.Name.ToLower().Contains(model.BookType.ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(model.Author))
            {
                products = products.Where(x => x.Author.Name.ToLower().Contains(model.Author.ToLower())).ToList();
            }
            if (model.PriceFrom != null)
            {
                products = products.Where(x => x.Price >= model.PriceFrom).ToList();
            }
            if (model.PriceTo != null)
            {
                products = products.Where(x => x.Price <= model.PriceTo).ToList();
            }

            return products;
        }
    }
}