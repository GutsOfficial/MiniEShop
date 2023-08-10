using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace shopapp.webui.Models
{
    public class CartModel
    {
        public int CartId { get; set; }
        public List<CartItemModel> cartItems {get; set; }
        public double TotalPrice(){
            return cartItems.Sum(i=>i.Price*i.Quantity);
        }
    }
}