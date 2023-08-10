using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using shopapp.entity;

namespace shopapp.data.Abstract
{
    public interface IProductRepository:IRepository<Product>
    {
        Product GetProductDetails(string url);
        List<Product> GetProductByCategory(string name,int page,int pageSize);
        int GetCountByCategory(string category);
        List<Product> GetPopularProducts();
        List<Product> GetHomeProducts();
        Product GetByIdWithCategories(int id);
        List<Product> GetSearchResult(string key);
        void Update(Product entity, int[] categoryId);
    }
}