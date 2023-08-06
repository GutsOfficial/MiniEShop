using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using shopapp.webui.Models;
using shopapp.business.Abstract;

namespace shopapp.webui.Controllers;

public class HomeController : Controller
{
    
    private readonly IProductService repo; 
   public HomeController(IProductService _repo)
   {
    repo= _repo;
   }

    public IActionResult Index()
        {
            

             var productViewModel = new ProductViewModel()
            {
                Products = repo.GetAll()
            };

            return View(productViewModel);
        }

         // localhost:5000/home/about
        public IActionResult About()
        {
            return View();
        }

         public IActionResult Contact()
        {
            return View("MyView");
        }

    
}
