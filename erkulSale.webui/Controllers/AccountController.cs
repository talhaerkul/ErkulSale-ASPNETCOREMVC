using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using erkulSale.data;
using erkulSale.entity;
using erkulSale.webui.Extensions;
using erkulSale.webui.Identity;
using erkulSale.webui.Models;
using erkulSale.webui.Service;
using erkulSale.webui.EmailServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using erkulSale.business.Abstract;

namespace ErkulSale.Controllers
{

    [AutoValidateAntiforgeryToken]
    public class AccountController : Controller
    {
        private UserManager<User> UserManager;

        private SignInManager<User> SignInManager;

        private ICartService _cartService;

        private IEmailSender EmailSender;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IEmailSender emailSender, ICartService cartService)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            EmailSender = emailSender;
            _cartService = cartService;
        }
        public IActionResult Manage()
        {
            return View();
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Login(string ReturnUrl = null)
        {

            return View(new LoginModel()
            {
                ReturnUrl = ReturnUrl
            });
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByEmailAsync(model.Email);
            if (user is null)
            {
                ModelState.AddModelError("", "Geçerli bir mail hesabı girmelisiniz.");
                return View(model);
            }

            if (!await UserManager.IsEmailConfirmedAsync(user))
            {
                ModelState.AddModelError("", "Lütfen Mail Adresinize Gönderilen Linkten Hesabınızı Onaylayın.");
                return View(model);
            }

            var result = await SignInManager.PasswordSignInAsync(user, model.Password, true, true);
            if (result.Succeeded)
            {

                return Redirect(model.ReturnUrl ?? "~/"); // iki soru işareti null kontrolü yapar ve null ise sağındakini işler
            }
            ModelState.AddModelError("", "Geçerli bir kullanıcı adı veya parola girmelisiniz.");
            return View(model);
        }
        [HttpGet]
        public IActionResult Register()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = new User()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.UserName,
                Email = model.Email
            };

            var result = await UserManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                var code = await UserManager.GenerateEmailConfirmationTokenAsync(user);
                var url = Url.Action("ConfirmEmail", "Account", new
                {
                    userId = user.Id,
                    token = code
                });
                await EmailSender.SendEmailAsync(model.Email, "Hesabını Onayla", $"Lütfen Email Hesabınızı Onaylamak İçin Linke <a href='http://localhost:5099{url}'>Tıklayınız.</a>");

                return RedirectToAction("Login", "Account");
            }

            ModelState.AddModelError("", "Bilinmeyen Bir Hata Oluştu. Lütfen Tekrar Deneyiniz.");
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await SignInManager.SignOutAsync();
            return Redirect("~/");
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
            {
                TempData.Put("message", new AlertMessage("Hata Mesajı", "Geçersiz Token.", "danger"));

                return View();
            }

            var user = await UserManager.FindByIdAsync(userId);
            if (user != null)
            {
                var result = await UserManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                    _cartService.InitializeCart(userId);
                    TempData.Put("message", new AlertMessage("Bildirme Mesajı", "Hesabınız Onaylandı.", "success"));

                    return View();
                }
            }


            TempData.Put("message", new AlertMessage("Uyarı Mesajı", "Hesabınız Onaylanırken Bir Hata Oluştu.", "warning"));

            return View();
        }
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string Email)
        {
            if (string.IsNullOrEmpty(Email))
            {
                TempData.Put("message", new AlertMessage("Uyarı Mesajı", "Email Adresi Girin.", "warning"));
                return View();
            }
            try
            {
                var user = await UserManager.FindByEmailAsync(Email);
                var code = await UserManager.GeneratePasswordResetTokenAsync(user);
                var url = Url.Action("ResetPassword", "Account", new
                {
                    userId = user.Id,
                    token = code
                });
                await EmailSender.SendEmailAsync(Email, "Parola Yenile", $"Yeni Parola Oluşturmak İçin Linke <a href='http://localhost:5099{url}'>Tıklayınız.</a>");
                TempData.Put("message", new AlertMessage("Bildirme Mesajı", "Parola Yenileme Maili Gönderildi.", "success"));


                return RedirectToAction("Index", "Home");
            }
            catch (Exception e)
            {
                TempData.Put("message", new AlertMessage("Hata Mesajı", "Hatalı Email Adresi.", "danger"));

                return View(Email);
            }
        }
        [HttpGet]
        public IActionResult ResetPassword(string userId, string token)
        {
            if (userId == null || token == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var model = new ResetPasswordModel { Token = token, UserId = userId };
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var result = await UserManager.ResetPasswordAsync(user, model.Token, model.Password);

            if (result.Succeeded)
            {
                TempData.Put("message", new AlertMessage("Bildirme Mesajı", "Parola Yenilendi.", "success"));
                return RedirectToAction("Login", "Account");
            }

            return View(model);
        }

    }
}