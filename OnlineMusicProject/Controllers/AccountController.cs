using OnlineMusicProject.Services.IServices;
using OnlineMusicProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineMusicProject.Models;
using OnlineMusicProject.ViewModels;
using System.Security.Principal;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.Win32;

namespace OnlineMusicProject.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<Users> signInManager;
        private readonly UserManager<Users> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;
        private readonly IJwtService _jWT;
        private readonly IMail _mail;

        public AccountController(SignInManager<Users> signInManager, UserManager<Users> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, IJwtService jWT, IMail mail)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.roleManager = roleManager;
            _configuration = configuration;
            _jWT = jWT;
            _mail = mail;
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError(nameof(model.Email), "Email không tồn tại.");
                return View(model);
            }
            var isComfirmEmail = await userManager.IsEmailConfirmedAsync(user);
            if (!isComfirmEmail)
            {
                TempData["SendAgain"] = "Tài khoản của bạn chưa được xác thực email.";
                ViewBag.UserId = user.Id;
                var decode = await userManager.GenerateEmailConfirmationTokenAsync(user);
                ViewBag.Token = decode;
                Console.WriteLine("No send: " + user.Id);
                Console.WriteLine("No send: " + decode);
                return View(model);
            }
            await userManager.ResetAccessFailedCountAsync(user);

            var token = await _jWT.GenerateTokenJWT(user);

            Response.Cookies.Append("access_Token", token.Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Expires = token.ExpriseAt
            });

            Response.Cookies.Append("refresh_Token", token.RefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Expires = DateTime.UtcNow.AddHours(1)
            });
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("Password", "Email or Password is incorrect");
                    return View(model);
                }
            }
            return View(model);
        }
       
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            var isEmail = await userManager.FindByEmailAsync(model.Email);
            if (isEmail != null)
            {
                ModelState.AddModelError("Email", "Email đã tồn tại");
                return View(model);
            }

            if (ModelState.IsValid)
            {
                Users user = new Users
                {
                    FullName = model.FullName,
                    Email = model.Email,
                    UserName = model.Email
                };
                var result = await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    var roleExit = await roleManager.RoleExistsAsync("User");
                    if (!roleExit)
                    {
                        var role = new IdentityRole("User");
                        await roleManager.CreateAsync(role);
                    }
                    await userManager.AddToRoleAsync(user, "User");
                    await signInManager.SignInAsync(user, isPersistent: false);
                    // Validate
                    var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    var confirmationLink = Url.Action("ConfirmEmail", "Account",
                        new { userId = user.Id, token = token }, Request.Scheme);
                    Console.WriteLine("Code: " + token);
                    // send Mail
                    await _mail.SendMailConfirmAsync(model.Email, confirmationLink);
                    await userManager.AddToRoleAsync(user, "User");
                    TempData["Success"] = "Đăng ký thành công, vui lòng kiểm tra email để xác thực tài khoản.";
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(model);
                }
            }
            return View(model);
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                TempData["SendAgain"] = "Người dùng không tồn tại.";
                return RedirectToAction("Login");
            }
            var result = await userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    Console.WriteLine("Lỗi: " + error.Description);
                }
                return Redirect("~/Status/Fail.html");
            }

            return Redirect("~/Status/Success.html");
        }

        [HttpPost]
        public async Task<IActionResult> SendMailAgain(string userId, string token)
        {

            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                TempData["SendAgain"] = "Thông tin xác thực không hợp lệ.";
                return RedirectToAction("Login");
            }
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                TempData["SendAgain"] = "Người dùng không tồn tại.";
                return RedirectToAction("Login");
            }
            var confirmationLink = Url.Action("ConfirmEmail", "Account",
               new { userId = userId, token = token }, Request.Scheme);
            await _mail.SendMailConfirmAsync(user.Email, confirmationLink);

            return View("login");
        }
        public IActionResult VerifyEmail()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> VerifyEmail(VerifyEmailViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("Email", "Email không tồn tại.");
                return View(model);
            }
            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            var resetLink = Url.Action("ChangePassword", "Account",
                               new { token, email = user.Email }, Request.Scheme);
            await _mail.SendMailForgotPassWord(user.Email, resetLink);
            TempData["Success"] = "Vui lòng kiểm tra email để đặt lại mật khẩu.";
            return RedirectToAction("Login");
        }
        [HttpGet]
        public async Task<IActionResult> ChangePassword(string token, string email)
        {
            if (token == null || email == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var model = new ChangePasswordViewModel { Token = token, Email = email };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return RedirectToAction("VerifyEmail");
            }

            var result = await userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
            if (result.Succeeded)
            {
                TempData["Success"] = "Mật khẩu của bạn đã được đặt lại thành công!";
                return RedirectToAction("Login");
            }
            foreach (var error in result.Errors)
            {
                if (error.Code.Contains("NewPassword"))
                {
                    ModelState.AddModelError(nameof(model.NewPassword), error.Description);
                }
                Console.WriteLine("Error: " + error);
            }
            return View(model);
        }
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            Response.Cookies.Delete("access_Token");
            Response.Cookies.Delete("refresh_Token");
            var refreshToken = Request.Cookies["refresh_Token"];
            await _jWT.RemoveRefreshTokenAsync(refreshToken);
            return RedirectToAction("Login");
        } 
    }
}
