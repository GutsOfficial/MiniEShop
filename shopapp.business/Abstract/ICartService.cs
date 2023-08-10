using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using shopapp.entity;
namespace shopapp.business.Abstract
{
    public interface ICartService
    {
        void InitializeCart(string userId);
        void AddToCart(string userId, int productId,int Quantity);
        void  DeleteFromCart(string userId,int productId);
        Cart GetCartByUserId(string userId);
    }
}