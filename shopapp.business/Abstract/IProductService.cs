using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using shopapp.entity;

namespace shopapp.business.Abstract
{
    public interface IProductService
    {
        Product GetById(int id);
        Product GetProductDetails(string url);
        List<Product> GetProductByCategory(string name,int page,int pageSize);
        List<Product> GetAll();
        void Create(Product entity);
        void Update(Product entity);
        void Update(Product entity, int[] categoryId);
        void Delete(Product entity);
        Product GetByIdWithCategories(int id);
        int GetCountByCategory( string category);
         List<Product> GetHomeProducts();
         List<Product> GetSearchResult(string key);
    }
}