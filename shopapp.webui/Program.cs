using shopapp.business.Abstract;
using shopapp.business.Concrete;
using shopapp.data.Abstract;
using shopapp.data.Concrete.EfCore;
using shopapp.business.Abstract;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<IProductRepository, EfProductRepo>();

builder.Services.AddTransient<ICategoryRepository, EfCatogeryRepo>();


builder.Services.AddTransient<ICategoryService, CategoryManager>();

builder.Services.AddTransient<IProductService, ProductManager>();

// Add services to the container.

builder.Services.AddControllersWithViews();
var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseEndpoints(endpoints=>{

endpoints.MapControllerRoute(
        name:"adminproducts",
        pattern:"admin/products",
        defaults:new {controller="Admin",action="productlist"}
    );
    endpoints.MapControllerRoute(
       name: "adminproductlist",
       pattern: "admin/products/{id?}",
       defaults: new { controller = "Admin", action = "edit" }
   );
    endpoints.MapControllerRoute(
      name: "adminproducts",
      pattern: "admin/products/create",
      defaults: new { controller = "Admin", action = "CreateProduct" }
  );


    // admin category route
    endpoints.MapControllerRoute(
        name: "adminproducts",
        pattern: "admin/categories",
        defaults: new { controller = "Admin", action = "categorylist" }
    );
    endpoints.MapControllerRoute(
       name: "adminproducts",
       pattern: "admin/categories/{id?}",
       defaults: new { controller = "Admin", action = "categoryedit" }
   );
    endpoints.MapControllerRoute(
       name: "adminproducts",
       pattern: "admin/categories/create",
       defaults: new { controller = "Admin", action = "CategoryCreate" }
   );
   

  endpoints.MapControllerRoute(
        name:"search",
        pattern:"search",
        defaults:new {controller="Shop",action="search"}
    );

   
    
    
    endpoints.MapControllerRoute(
        name:"products",
        pattern:"products/{category?}",
        defaults:new {controller="Shop",action="list"}
    );
});
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
