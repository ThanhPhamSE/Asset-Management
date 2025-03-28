using Asset_Management.Models;
using Asset_Management.Services.IServices;
using Asset_Management.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Asset_Management.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<Users> signInManager;
        private readonly UserManager<Users> userManager;
        private readonly IEmailService _emailService;

        public AccountController(SignInManager<Users> signInManager, UserManager<Users> userManager,IEmailService emailService)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            _emailService = emailService;
        }

        public IActionResult Login()
        {
            return View();
        }

        //[HttpPost]
        //public async Task<IActionResult> Login(LoginViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
        //        if (result.Succeeded)
        //        {
        //            return RedirectToAction("Index", "Home");
        //        }
        //        else
        //        {
        //            ModelState.AddModelError("","Email or password is incorrect");
        //            return View(model);
        //        }
        //    }
        //    return View(model);
        //}

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError("", "Email or password is incorrect");
                    return View(model);
                }

                if (user.LockoutEnd != null && user.LockoutEnd > DateTime.UtcNow)
                {
                    ModelState.AddModelError("", "Your account is inactive. Please contact support.");
                    return View(model);
                }

                var result = await signInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("List", "Asset");
                }
                else
                {
                    ModelState.AddModelError("", "Email or password is incorrect");
                    return View(model);
                }
            }
            return View(model);
        }

        public IActionResult VerifyEmail()
        {
            return View();
        }

        //[HttpPost]
        //public async Task<IActionResult> VerifyEmail(VerifyEmailViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = await userManager.FindByEmailAsync(model.Email);
        //        if(user == null)
        //        {
        //            ModelState.AddModelError("", "Something is wrong!");
        //            return View(model);
        //        }
        //        else
        //        {
        //            return RedirectToAction("ChangePassword", "Account", new {email = user.Email });
        //        }
        //    }
        //    return View(model);
        //}
        [HttpPost]
        public async Task<IActionResult> VerifyEmail(VerifyEmailViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError("", "Email không tồn tại!");
                    return View(model);
                }

                // Tạo token để đổi mật khẩu
                var token = await userManager.GeneratePasswordResetTokenAsync(user);

                // Tạo URL để gửi trong email
                var resetLink = Url.Action("ChangePassword", "Account", new { email = user.Email, token = token }, Request.Scheme);

                // Gửi email
                var message = new Message(new string[] { user.Email }, "Password Reset", $"Click vào đây để đặt lại mật khẩu: <a href='{resetLink}'>Đổi mật khẩu</a>");
                _emailService.SendEmail(message);

                TempData["SuccessSendMessage"] = "Liên kết đặt lại mật khẩu đã được gửi đến email của bạn.";
                return View();
            }
            return View(model);
        }


        //public IActionResult ChangePassword(string email)
        //{
        //    if (string.IsNullOrEmpty(email))
        //    {
        //        return RedirectToAction("VerifyEmail", "Account");
        //    }
        //    return View(new ChangePasswordViewModel { Email = email});
        //}

        //[HttpPost]
        //public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = await userManager.FindByEmailAsync(model.Email);
        //        if (user != null)
        //        {
        //            var result = await userManager.RemovePasswordAsync(user);
        //            if (result.Succeeded) {
        //                result = await userManager.AddPasswordAsync(user, model.NewPassword);
        //                return RedirectToAction("Login", "Account"); 
        //            }
        //            else
        //            {
        //                foreach (var error in result.Errors)
        //                {
        //                    ModelState.AddModelError("", error.Description);
        //                }
        //                return View(model);
        //            }
        //        }
        //        else
        //        {
        //            ModelState.AddModelError("", "Email not found!");
        //            return View(model);
        //        }
        //    }
        //    else
        //    {
        //        ModelState.AddModelError("", "Something went wrong.try again.");
        //        return View(model);
        //    }
        //}
        public IActionResult ChangePassword(string email, string token)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
            {
                return RedirectToAction("VerifyEmail", "Account");
            }

            return View(new ChangePasswordViewModel { Email = email, Token = token });
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                                 .Select(e => e.ErrorMessage)
                                 .ToList();
                TempData["ErrorMessage"] = string.Join("<br>", errors);
                return View(model);
            }

            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("", "Email không hợp lệ!");
                return View(model);
            }

            var result = await userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
            if (result.Succeeded)
            {
                TempData["SuccessChangePasswordMessage"] = "Mật khẩu đã được đặt lại thành công!";
                return RedirectToAction("Login", "Account");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}
