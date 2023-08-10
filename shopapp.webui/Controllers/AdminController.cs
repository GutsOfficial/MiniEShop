using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using shopapp.business.Abstract;
using shopapp.webui.Models;
using shopapp.entity;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using shopapp.webui.Identity;

namespace shopapp.webui.Controllers
{
    
    [Authorize]
    public class AdminController : Controller
    {
        private readonly IProductService repo;
        private readonly ICategoryService categoryrepo;
        private RoleManager<IdentityRole> roleManager;
        private UserManager<User> userManager;
        public AdminController(IProductService _repo, ICategoryService _categoryrepo,RoleManager<IdentityRole> _roleManager,UserManager<User> _userManager)
   {
    repo= _repo;
            categoryrepo= _categoryrepo;
            roleManager=_roleManager;
            userManager=_userManager;
   }
        public IActionResult RoleList(){
            return View(roleManager.Roles);
        }
         public IActionResult RoleCreate(){
            return View();
        }
        [HttpPost]
         public async Task<IActionResult> RoleCreate(RoleModel model){
            var result = await roleManager.CreateAsync(new IdentityRole(model.Name));
            if(result.Succeeded){
                return RedirectToAction("RoleList");
            }
            return View(model);
        }
         public async Task<IActionResult> RoleEdit(string id)
        {   
            var role = await roleManager.FindByIdAsync(id);
            var members=new List<User>();
            var nonmembers = new List<User>();
            foreach (var item in userManager.Users)
            {
                 var list= await userManager.IsInRoleAsync(item,role.Name)?members:nonmembers;
                 list.Add(item);
            }
            var model = new RoleDetails(){
                Role=role,
                Members=members,
                NonMembers=nonmembers
            };
            return View(model);
        }
        [HttpPost]
         public async Task<IActionResult> RoleEdit(RoleEditModel model)
        {   
            foreach(var item in  model.IdsToAdd?? new string[]{}){
                var user = await userManager.FindByIdAsync(item);
                if(item!=null){
                    var result = userManager.AddToRoleAsync(user,model.RoleName);
                    
                }
            }

             foreach(var item in  model.IdsToDelete?? new string[]{}){
                var user = await userManager.FindByIdAsync(item);
                if(item!=null){
                    var result = userManager.RemoveFromRoleAsync(user,model.RoleName);
                    
                }
               
            }
             return Redirect("/admin/role/"+model.RoleId);
        }
        public IActionResult UserList()
        {
            return View(userManager.Users);
        }
        public async Task<IActionResult> UserEdit(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if(user!=null){
                var selectedRoles= await userManager.GetRolesAsync(user);
                var roles=roleManager.Roles.Select(i=>i.Name);
                ViewBag.Roles=roles;
                return View(new UserDetails(){
                    UserId=user.Id,
                    FirstName=user.FirstName,
                    LastName=user.LastName,
                    UserName=user.UserName,
                    Email=user.Email,
                    EmailConfirmed=user.EmailConfirmed,
                    SelectedRoles= selectedRoles
                });
            }
            return Redirect("~/admin/user/list");
        }
        [HttpPost]
        public async Task<IActionResult> UserEdit(UserDetails model,string[] selectedRoles)
        {
            var user = await userManager.FindByIdAsync(model.UserId);

            user.FirstName=model.FirstName;
            user.LastName=model.LastName;
            user.UserName=model.UserName;
            user.Email=model.Email;
            user.EmailConfirmed=model.EmailConfirmed;

            var result = await userManager.UpdateAsync(user);
            if(result.Succeeded){
            var userRoles= await userManager.GetRolesAsync(user);
            selectedRoles= selectedRoles?? new string[]{};
            await userManager.AddToRolesAsync(user,selectedRoles.Except(userRoles).ToArray<string>());
            await userManager.RemoveFromRolesAsync(user,userRoles.Except(selectedRoles).ToArray<string>());
            return Redirect("~/admin/user/list");
            }
            




             return Redirect("~/admin/user/list");
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
            var entity = repo.GetByIdWithCategories((int)id);
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
                kategoriler=entity.productcategory.Select(i=>i.kategori).ToList()

            };
            ViewBag.Categories = categoryrepo.GetAll();
            return View(model);
        }
         [HttpPost]
        public async Task<IActionResult> Edit(ProductModel product , int[] categoryId,IFormFile file){
            var entity = repo.GetById(product.ProductId);
            entity.Name=product.Name;
            entity.Description=product.Description;
            entity.Price=product.Price;
            
            entity.Url=product.Url;
            if(file!=null){
                entity.ImageUrl=file.FileName;
                var extension= Path.GetExtension(file.FileName);
                var randomName=string.Format($"{DateTime.Now.Ticks}{extension}");
                entity.ImageUrl=randomName;
                var path = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot\\img",randomName);

                using(var stream = new FileStream(path,FileMode.Create)){
                    await file.CopyToAsync(stream);
                }

            }
            repo.Update(entity,categoryId);
            
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
        [HttpGet]
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
            var entity = categoryrepo.GetByIdWithProduct((int)id);
            if (entity == null)
            {
                return NotFound();
            }
            var model = new CategoryModel()
            {
               CategoryId= entity.CategoryId,
                Name = entity.Name,              
                Url = entity.Url,
                urunler=entity.productcategory.Select(p=>p.urun).ToList()

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
         [HttpPost]
        public IActionResult DeleteFromCategory(int categoryId,int productId){
                    categoryrepo.DeleteFromCategory(categoryId,productId);
                    return Redirect("/admin/categories/"+categoryId);
        }

        
        
    }
    
}