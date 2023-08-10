using shopapp.business.Abstract;
using shopapp.business.Concrete;
using shopapp.data.Abstract;
using shopapp.data.Concrete.EfCore;
using shopapp.business.Abstract;
using shopapp.webui.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using shopapp.webui.EmailServices;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

builder.Services.AddDbContext<ApplicationContext>(options=>options.UseSqlite("Data Source=shopDb"));
builder.Services.AddIdentity<User,IdentityRole>().AddEntityFrameworkStores<ApplicationContext>().AddDefaultTokenProviders();
builder.Services.AddTransient<IProductRepository, EfProductRepo>();
builder.Services.Configure<IdentityOptions>(options=> {
                // password
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = true;

                // Lockout                
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.AllowedForNewUsers = true;

                // options.User.AllowedUserNameCharacters = "";
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;
            });
builder.Services.ConfigureApplicationCookie(options=> {
                options.LoginPath = "/account/login";
                options.LogoutPath = "/account/logout";
                options.AccessDeniedPath = "/admin/accessdenied";
                options.SlidingExpiration = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                options.Cookie = new CookieBuilder
                {
                    HttpOnly = true,
                    Name = ".ShopApp.Security.Cookie",
                    SameSite =SameSiteMode.Strict
                };
            });
builder.Services.AddTransient<ICategoryRepository, EfCatogeryRepo>();
builder.Services.AddTransient<ICartRepository, EfCartRepo>();

builder.Services.AddTransient<ICategoryService, CategoryManager>();
builder.Services.AddTransient<ICartService, CartManager>();
builder.Services.AddTransient<IProductService, ProductManager>();
builder.Services.AddTransient<IEmailSender, SmtpEmailSender>(i=>new SmtpEmailSender(
        config["EmailSender:Host"],
        config.GetValue<int>("EmailSender:port"),
        config.GetValue<bool>("EmailSender:EnableSSL"),
        config["EmailSender:UserName"],
        config["EmailSender:Password"]



));

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
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints=>{

  endpoints.MapControllerRoute(
        name:"checkout",
        pattern:"cart",
        defaults:new {controller="Cart",action="CheckOut"}
    );
     endpoints.MapControllerRoute(
        name:"cart",
        pattern:"cart",
        defaults:new {controller="Cart",action="Index"}
    );


//admin actions

    endpoints.MapControllerRoute(
        name:"adminrusers",
        pattern:"admin/user/list",
        defaults:new {controller="Admin",action="UserList"}
    );
    endpoints.MapControllerRoute(
        name:"adminUserEdit",
        pattern:"admin/user/{id?}",
        defaults:new {controller="Admin",action="UserEdit"}
    );
    


//role actions
    endpoints.MapControllerRoute(
        name:"adminroles",
        pattern:"admin/role/list",
        defaults:new {controller="Admin",action="RoleList"}
    );
    endpoints.MapControllerRoute(
        name:"adminrolecreate",
        pattern:"admin/role/create",
        defaults:new {controller="Admin",action="RoleCreate"}
    );
    endpoints.MapControllerRoute(
        name:"adminroleEdit",
        pattern:"admin/role/{id?}",
        defaults:new {controller="Admin",action="RoleEdit"}
    );




//products
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
    name: "adminproducts2",
    pattern: "admin/products/create",
    defaults: new { controller = "Admin", action = "CreateProduct" }
);


    // admin category route
   
    endpoints.MapControllerRoute(
        name: "adminproducts3",
        pattern: "admin/categories",
        defaults: new { controller = "Admin", action = "categorylist" }
    );
    endpoints.MapControllerRoute(
       name: "adminproducts4",
       pattern: "admin/categories/{id?}",
       defaults: new { controller = "Admin", action = "categoryedit" }
   );
    endpoints.MapControllerRoute(
     name: "adminproducts5",
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
