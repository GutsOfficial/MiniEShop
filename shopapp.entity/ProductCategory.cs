using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace shopapp.entity
{
    public class ProductCategory
    {
        public int CategoryId { get; set; }
        public Category kategori { get; set; }
        public int ProductId { get; set; }
        public Product urun { get; set; }
    }
}