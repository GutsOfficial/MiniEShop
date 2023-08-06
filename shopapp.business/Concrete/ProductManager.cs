using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using shopapp.business.Abstract;
using shopapp.data.Abstract;
using shopapp.data.Concrete.EfCore;
using shopapp.entity;

namespace shopapp.business.Concrete
{
    public class ProductManager : IProductService
    {
        private IProductRepository repo;
        public ProductManager(IProductRepository _repo)
        {
            repo=_repo;
        }
        public void Create(Product entity)
        {
            repo.Create(entity);
        }

        public void Delete(Product entity)
        {
            repo.Delete(entity);
        }

        public List<Product> GetAll()
        {
           return repo.GetAll();
        }
        public List<Product> GetSearchResult(string key){
                return repo.GetSearchResult(key);
        }


        public Product GetById(int id)
        {
            return repo.GetById(id);
        }
        
        public List<Product> GetProductByCategory(string name,int page,int pageSize){
            return repo.GetProductByCategory(name, page, pageSize);
        }
        
        public  Product GetProductDetails(string url){
            return repo.GetProductDetails(url);
        }
        public void Update(Product entity)
        {
             repo.Update(entity);
        }
       public int GetCountByCategory(string category){
            return repo.GetCountByCategory(category);
        }

         public List<Product> GetHomeProducts(){
            return repo.GetHomeProducts();
         }
    }
}