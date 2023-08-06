using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using shopapp.business.Abstract;

namespace shopapp.webui.ViewComponents
{
    public class CategoriesViewComponent:ViewComponent
    {
         private readonly ICategoryService repo; 
   public CategoriesViewComponent(ICategoryService _repo)
   {
    repo= _repo;
   }
        public IViewComponentResult Invoke()
        {
             if (RouteData.Values["category"]!=null)
                 ViewBag.SelectedCategory = RouteData?.Values["category"];
            return View(repo.GetAll());
            
        }
    }
}