using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using shopapp.business.Abstract;
using shopapp.webui.Models;
using shopapp.entity;
using Newtonsoft.Json;
namespace shopapp.webui.Controllers
{
    
    public class AdminController : Controller
    {
        private readonly IProductService repo;
        private readonly ICategoryService categoryrepo;
        public AdminController(IProductService _repo, ICategoryService _categoryrepo)
   {
    repo= _repo;
            categoryrepo= _categoryrepo;
   }
        public IActionResult ProductList()
        {
            return View(new ProductViewModel(){
                Products = repo.GetAll()
            });
        }

        public IActionResult CreateProduct(){
            return View();
        }
        [HttpPost]
        public IActionResult CreateProduct(ProductModel product){
            var entity = new Product(){

                Name= product.Name,
                Url=product.Url,
                Price=product.Price,
                Description=product.Description,
                ImageUrl=product.ImageUrl
            };
            repo.Create(entity);
            var msg = new AlertMessage()
            {
                Message = $"{entity.Name} isimli ürün eklendi.",
                AlertType = "sucess"
            };
            TempData["message"] = JsonConvert.SerializeObject(msg);
            return RedirectToAction("ProductList");
        }
        public IActionResult Edit(int id){
            if(id==null){
                return NotFound();
            }
            var entity = repo.GetById((int)id);
            if(entity==null){
                return NotFound();
            }
            var model = new ProductModel(){
                ProductId=entity.ProductId,
                Name=entity.Name,
                Description=entity.Description,
                Price=entity.Price,
                ImageUrl=entity.ImageUrl,
                Url=entity.Url,

            };
            return View(model);
        }
         [HttpPost]
        public IActionResult Edit(ProductModel product){
            var entity = repo.GetById(product.ProductId);
            entity.Name=product.Name;
            entity.Description=product.Description;
            entity.Price=product.Price;
            entity.ImageUrl=product.ImageUrl;
            entity.Url=product.Url;
            repo.Update(entity);
            var msg = new AlertMessage()
            {
                Message = $"{entity.Name} isimli ürün güncellendi.",
                AlertType = "success"
            };
            TempData["message"] = JsonConvert.SerializeObject(msg);
            return RedirectToAction("ProductList");
        }
        [HttpPost]
        public IActionResult DeleteProduct(int productId){
            var entity = repo.GetById(productId);
            repo.Delete(entity);
            var msg = new AlertMessage()
            {
                Message = $"{entity.Name} isimli ürün silindi.",
                AlertType = "danger"
            };
            TempData["message"] = JsonConvert.SerializeObject(msg);
            return RedirectToAction("ProductList");
        }
        //category part

        public IActionResult CategoryList()
        {
            return View(new CategoryViewModel()
            {
                kategoriler = categoryrepo.GetAll()
            }) ;
        }

        public IActionResult CategoryCreate()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CategoryCreate(CategoryModel category)
        {
            var entity = new Category()
            {

                Name = category.Name,
               Url=category.Url
            };
            categoryrepo.Create(entity);
            var msg = new AlertMessage()
            {
                Message = $"{entity.Name} isimli kategori eklendi.",
                AlertType = "sucess"
            };
            TempData["message"] = JsonConvert.SerializeObject(msg);
            return RedirectToAction("CategoryList");
        }
        public IActionResult CategoryEdit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var entity = categoryrepo.GetById((int)id);
            if (entity == null)
            {
                return NotFound();
            }
            var model = new CategoryModel()
            {
               CategoryId= entity.CategoryId,
                Name = entity.Name,              
                Url = entity.Url,

            };
            return View(model);
        }
        [HttpPost]
        public IActionResult CategoryEdit(CategoryModel category)
        {
            var entity = categoryrepo.GetById(category.CategoryId);
            entity.Name = category.Name;
            
            entity.Url = category.Url;
            categoryrepo.Update(entity);
            var msg = new AlertMessage()
            {
                Message = $"{entity.Name} isimli kategori güncellendi.",
                AlertType = "success"
            };
            TempData["message"] = JsonConvert.SerializeObject(msg);
            return RedirectToAction("CategoryList");
        }
        [HttpPost]
        public IActionResult DeleteCategory(int categoryId)
        {
            var entity = categoryrepo.GetById(categoryId);
            categoryrepo.Delete(entity);
            var msg = new AlertMessage()
            {
                Message = $"{entity.Name} isimli kategori silindi.",
                AlertType = "danger"
            };
            TempData["message"] = JsonConvert.SerializeObject(msg);
            return RedirectToAction("CategoryList");
        }
    }
    
}