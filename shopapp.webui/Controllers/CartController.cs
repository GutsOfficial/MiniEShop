using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using shopapp.business.Abstract;
using shopapp.webui.Identity;
using Microsoft.AspNetCore.Identity;
using shopapp.webui.Models;
using shopapp.entity;

namespace shopapp.webui.Controllers
{
   [Authorize]
    public class CartController : Controller
    {
       private ICartService cartService;
       private UserManager<User> userManager;
       public CartController(ICartService _cartService,UserManager<User> _userManager)
       {
        userManager=_userManager;
        cartService=_cartService;
       }

        public IActionResult Index()
        {
            var cart = cartService.GetCartByUserId(userManager.GetUserId(User));
            
           return View(new CartModel(){
                CartId = cart.Id,
                cartItems = cart.sepetItems.Select(i=>new CartItemModel()
                {
                    CartItemId = i.CartItemId,
                    ProductId = i.ProductId,
                    Name = i.urun.Name,
                    Price = (double)i.urun.Price,
                    ImageUrl = i.urun.ImageUrl,
                    Quantity =i.Quantity

                }).ToList()
            });
        }
        
        [HttpPost]
         public IActionResult AddToCart(int productId,int quantity)
        {
            var userId =userManager.GetUserId(User);
            cartService.AddToCart(userId,productId,quantity);
            return Redirect("~/cart");
        }
        [HttpPost]
         public IActionResult DeleteFromCart(int productId)
        {
             var userId =userManager.GetUserId(User);
            cartService.DeleteFromCart(userId,productId);
            return Redirect("~/cart");
        }
        
        
        
    }
}