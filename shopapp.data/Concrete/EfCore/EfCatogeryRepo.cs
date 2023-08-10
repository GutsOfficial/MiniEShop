using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using shopapp.data.Abstract;
using shopapp.entity;

namespace shopapp.data.Concrete.EfCore
{
    public class EfCatogeryRepo : EfGenericRepo<Category,ShopContext>,ICategoryRepository
    {
        public Category GetByIdWithProduct(int categoryId)
        {
            using (var context = new ShopContext())
            {
                return context.Categories.Where(i => i.CategoryId == categoryId)
                    .Include(i => i.productcategory)
                    .ThenInclude(i => i.urun)
                    .FirstOrDefault();
            }
        }
        public void DeleteFromCategory(int categoryId, int productId){
             using (var context = new ShopContext())
            {
                var cmd = "delete from productcategory where ProductId=@p0  and CategoryId=@p1";
                context.Database.ExecuteSqlRaw(cmd,productId,categoryId);
                context.SaveChanges();
            }
        }
    }
}