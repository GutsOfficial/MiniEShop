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

namespace shopapp.webui.Controllers
{
    
    public class ShopController : Controller
    {
        
        private readonly IProductService repo; 
        public ShopController(IProductService _repo)
        {
                repo= _repo;
        }



         public IActionResult List(string category,int page=1)
        {
            
           const int pageSize=3;
             var productViewModel = new ProductViewModel()
            {
                PageInfo = new PageInfo(){
                    TotalItems= repo.GetCountByCategory(category),
                    CurrentPage=page,
                    ItemsPerPage=pageSize,
                    CurrentCategory=category
                },
                Products = repo.GetProductByCategory(category,page, pageSize)
            };

            return View(productViewModel);
        }


        //details
        public IActionResult Details(string url){
            if(url==null){
                return NotFound();
            }
            Product product = repo.GetProductDetails(url);
            if(product==null){
                return NotFound();
            }
            return View(new ProductDetailModel{
                urun=product,
                kategoriler= product.productcategory.Select(i=>i.kategori).ToList()
            });
        }
        public IActionResult Search(string key ){
             
             var productViewModel = new ProductViewModel()
            {
                
                Products = repo.GetSearchResult(key)
            };

            return View(productViewModel);
        }
       
    }
}