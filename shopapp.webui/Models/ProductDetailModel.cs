using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using shopapp.entity;

namespace shopapp.webui.Models
{
    public class ProductDetailModel
    {
        public Product urun { get; set; }
        public List<Category> kategoriler { get; set; }
    }
}