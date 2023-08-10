using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using shopapp.entity;

namespace shopapp.webui.Models
{
    public class ProductModel
    {
         public int ProductId { get; set; }
        
        
        public string Name { get; set; } 
        public string Url { get; set; }
       
        public double? Price { get; set; } 
        public string Description { get; set; } 
         
        public string ImageUrl { get; set; }
        public bool IsApproved { get; set; }
        public List<Category> kategoriler { get; set; }
    }
}
