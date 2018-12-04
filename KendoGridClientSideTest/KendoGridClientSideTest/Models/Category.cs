using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace KendoGridClientSideTest.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Product> Products { get; set; }
        public Category()
        {
            Products = new Collection<Product>();
        }
    }
}