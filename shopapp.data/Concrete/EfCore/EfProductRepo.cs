using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using shopapp.data.Abstract;
using shopapp.entity;
using Microsoft.EntityFrameworkCore;

namespace shopapp.data.Concrete.EfCore
{
    public class EfProductRepo : EfGenericRepo<Product, ShopContext>, IProductRepository
    {
        public Product GetProductDetails(string url){
            using(var context = new ShopContext()){
                return context.Products.Where(i=>i.Url==url).Include(i=>i.productcategory).ThenInclude(i=>i.kategori).FirstOrDefault();
            }
             
        }
       public List<Product> GetProductByCategory(string name,int page, int pageSize){

                using(var context = new ShopContext()){
                var products= context.Products.AsQueryable();
                if (!string.IsNullOrEmpty(name))
                {
                    products = products.Include(i=>i.productcategory)
                    .ThenInclude(i=>i.kategori)
                    .Where(i=>i.productcategory
                    .Any(a=>a.kategori.Url==name));
                }
                return products.Skip((page-1)*pageSize).Take(pageSize).ToList();
            }
        }
        public List<Product> GetSearchResult(string key)
        {
              using(var context = new ShopContext()){
                var products= context.Products.Where(i=>i.Name.ToLower().Contains(key.ToLower()) || i.Description.ToLower().Contains(key.ToLower())).AsQueryable();
                
                return products.ToList();
            }
        }
        public List<Product> GetPopularProducts()
        {
            throw new NotImplementedException();
        }
        public List<Product> GetHomeProducts()
        {
             throw new NotImplementedException();
        }

       public int GetCountByCategory(string category){
            using(var context = new ShopContext()){
                var products= context.Products.AsQueryable();
                if (!string.IsNullOrEmpty(category))
                {
                    products = products.Include(i=>i.productcategory)
                    .ThenInclude(i=>i.kategori)
                    .Where(i=>i.productcategory
                    .Any(a=>a.kategori.Url==category));
                }
                return products.Count();
             }
        
        }
            public Product GetByIdWithCategories(int id)
        {
             using(var context = new ShopContext()){
                return context.Products.Where(i=>i.ProductId==id)
                .Include(i=>i.productcategory)
                .ThenInclude(i=>i.kategori)
                .FirstOrDefault();
            }
        }
        public void Update(Product entity, int[] categoryId){
             using(var context = new ShopContext()){
               var  product = context.Products.Include(i=>i.productcategory).FirstOrDefault(i=>i.ProductId==entity.ProductId);
               if(product!=null){
                product.Name=entity.Name;
                product.Price=entity.Price;
                product.Description=entity.Description;
                product.ImageUrl=entity.ImageUrl;
                product.Url=entity.Url;
                product.productcategory= categoryId.Select(catid=>new ProductCategory(){
                        ProductId=entity.ProductId,
                        CategoryId=catid
                }).ToList();
                    context.SaveChanges();
               }

            }
        }
}
}