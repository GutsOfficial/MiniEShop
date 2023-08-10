using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using shopapp.business.Abstract;
using shopapp.data.Abstract;
using shopapp.entity;
namespace shopapp.business.Concrete
{
    public class CartManager:ICartService
    {
        private ICartRepository cartRepo;
        public CartManager(ICartRepository _cartRepo)
        {
            cartRepo=_cartRepo;
        }
        public void InitializeCart(string userId)
        {
            cartRepo.Create(new Cart(){UserId=userId});
        }      
        public   Cart GetCartByUserId(string userId){
            return cartRepo.GetCartByUserId(userId);
        }  
        public void AddToCart(string userId, int productId,int Quantity){
            var cart =GetCartByUserId(userId);
            if(cart!=null){
                var index = cart.sepetItems.FindIndex(i=>i.ProductId==productId);
                if(index<0){
                    cart.sepetItems.Add(new CartItem(){
                        ProductId=productId,
                        Quantity=Quantity,
                        CartId=cart.Id
                    });
                }else
                    {
                cart.sepetItems[index].Quantity+=Quantity;
                    }
            }
            cartRepo.Update(cart);
        }
        public void  DeleteFromCart(string userId,int productId){

            var cart =GetCartByUserId(userId);
            if(cart!=null){
                cartRepo.DeleteFromCart(cart.Id,productId);
            }
            
        }
    }
}