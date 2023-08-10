using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using shopapp.webui.Identity;
using shopapp.webui.Models;
using Newtonsoft.Json;
using shopapp.webui.EmailServices;
using shopapp.business.Abstract;
namespace shopapp.webui.Controllers
{
   
    public class AccountController : Controller
    {
        private UserManager<User> userManager;
        private SignInManager<User> signInManager;
        private IEmailSender emailSender;
        private ICartService cartService;
        public AccountController(UserManager<User> _userManager,SignInManager<User> _signInManager,IEmailSender _emailSender,ICartService _cartService)
        {
            userManager=_userManager;
            signInManager=_signInManager;
            emailSender=_emailSender;
            cartService=_cartService;
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            var user = await userManager.FindByNameAsync(model.UserName);
            if(user==null){
                return View(model);
            }

            if(!await userManager.IsEmailConfirmedAsync(user)){
                 return RedirectToAction("fckuuuuuuuuuuuuuuuu");
            }
            var result = await signInManager.PasswordSignInAsync(user,model.Password,false,false);
            if(result.Succeeded){
                return RedirectToAction("Index","Home");
            }
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            var user = new User(){
                FirstName=model.FirstName,
                LastName=model.LastName,
                UserName=model.UserName,
                Email=model.Email
            };
            var result = await userManager.CreateAsync(user,model.Password);
            if(result.Succeeded){
                    var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    var url = Url.Action("ConfirmEmail","Account",new {
                        userId=user.Id,
                        token = code
                    });
                    cartService.InitializeCart(user.Id);
                    await emailSender.SendEmailAsync(model.Email,"Hesabınızı onaylayınız.",$"Lütfen email hesabınızı onaylamak için linkte <a href='https://localhost:5132{url}'>tıklayınız</a>");

                    
                    return RedirectToAction("Login","Account");
            }
            return View(model);
        }
        public async Task<IActionResult> LogOut(){
            await signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
         public async Task<IActionResult> ConfirmEmail(string userId,string token){

            if(userId==null||token==null){
                            var msg = new AlertMessage()
                        {
                            Message = "geçersiz token.",
                            AlertType = "danger"
                        };
                        TempData["message"] = JsonConvert.SerializeObject(msg);
               
                return View();
            }
            var user = await userManager.FindByIdAsync(userId);
            if(user==null){
                 var msg = new AlertMessage()
                        {
                            Message = "Böyle bir kullanıcı yok.",
                            AlertType = "danger"
                        };
                        TempData["message"] = JsonConvert.SerializeObject(msg);
                return View();
            }
            var result = await userManager.ConfirmEmailAsync(user,token);
            if(result.Succeeded){
                
                 var msg = new AlertMessage()
                        {
                            Message = "Hesap onaylandı.",
                            AlertType = "success"
                        };
                        TempData["message"] = JsonConvert.SerializeObject(msg);
                return View();
            }else{
                var msg = new AlertMessage()
                        {
                            Message = "Hesap onaylanmadı.",
                            AlertType = "danger"
                        };
                        TempData["message"] = JsonConvert.SerializeObject(msg);
                        return View();
            }
            
         }
         public IActionResult ForgotPassword(){
            return View();
         }
         [HttpPost]
         public IActionResult ForgotPassword(string email){
            return View();
         }
         public IActionResult AccessDenied(){
            return View();
        }
        
        
    }
}