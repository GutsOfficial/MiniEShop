using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace shopapp.entity
{
    public class CartItem
    {
        public int CartItemId { get; set; }
        public int ProductId { get; set; }
         public Product urun { get; set; }
        public int CartId { get; set; }
       public Cart sepet { get; set; }  
        public int Quantity { get; set; }
        
    }
}