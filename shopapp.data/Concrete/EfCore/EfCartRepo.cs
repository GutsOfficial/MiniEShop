using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using shopapp.entity;
using shopapp.data.Abstract;
using Microsoft.EntityFrameworkCore;

namespace shopapp.data.Concrete.EfCore
{
    public class EfCartRepo:EfGenericRepo<Cart,ShopContext>,ICartRepository 
    {
        public Cart GetCartByUserId(string userId){
            using(var context = new ShopContext()){
                return context.Carts.Include(i=>i.sepetItems).ThenInclude(i=>i.urun).FirstOrDefault(i=>i.UserId==userId);
            }
        }
         public override void Update(Cart entity)
        {
            using (var context = new ShopContext()){
                context.Carts.Update(entity);
                context.SaveChanges();
            }
        }
        public void DeleteFromCart(int cartId,int productId){
             using (var context = new ShopContext()){
                var cmd =@"delete from CartItems where CartId=@p0 and ProductId=@p1";
                context.Database.ExecuteSqlRaw(cmd,cartId,productId);

            }
        }
    }
}