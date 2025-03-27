using Asset_Management.Models;
using Asset_Management.Services.IServices;
using Asset_Management.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph.Models;

namespace Asset_Management.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        private readonly UserManager<Users> _userManager;
        private readonly RoleManager<Roles> _roleManager;

        public UserController(IUserService userService, UserManager<Users> userManager, RoleManager<Roles> roleManager)
        {
            _userService = userService;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> List()
        {
            var users = await _userService.GetAllUsersAsync();
            var roles = await _userService.GetRolesAsync();
            ViewBag.Roles = roles; // Truyền danh sách roles sang view
            return View(users);
        }

        public async Task<IActionResult> ToggleStatus(string id)
        {
            await _userService.ToggleUserStatusAsync(id);
            return RedirectToAction(nameof(List));
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(UserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                foreach (var error in errors)
                {
                    Console.WriteLine(error); // In lỗi ra console
                }
                ViewData["MessageAddUserFail"] = "Thêm người dùng không thành công. Vui lòng kiểm tra lại thông tin.";
                ViewBag.Roles = await _userService.GetRolesAsync();
                var users = await _userService.GetAllUsersAsync();
                return View("List", users);
            }

            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError("Email", "Email này đã được sử dụng. Vui lòng chọn email khác.");
                ViewData["MessageAddUserFail"] = "Thêm người dùng thất bại. Email đã tồn tại.";
                ViewBag.Roles = await _userService.GetRolesAsync();
                var users = await _userService.GetAllUsersAsync();
                return View("List", users);
            }

            var result = await _userService.AddUserAsync(model);
            //var savedUser = await _userManager.FindByEmailAsync(model.Email);
            if (!result)
            {
                ModelState.AddModelError("", "Failed to add user.");
                ViewData["MessageAddUserFail"] = "Thêm người dùng thất bại hãy kiểm tra lại thông tin.";
                ViewBag.Roles = await _userService.GetRolesAsync();
                var users = await _userService.GetAllUsersAsync();
                return View("List", users);
            }
            TempData["MessageAddUserSuccess"] = "Thêm người dùng thành công!";
            return RedirectToAction("List");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                TempData["MessageDeleteFail"] = "Xóa người dùng không thành công!";
                ModelState.AddModelError("", "Invalid user ID.");
                return RedirectToAction("List");
            }

            bool result = await _userService.DeleteUserAsync(userId);
            if (!result)
            {
                TempData["MessageDeleteFail"] = "Xóa người dùng không thành công!";
                ModelState.AddModelError("", "Failed to delete user.");
            }
            TempData["MessageDeleteUserSuccess"] = "Xóa người dùng thành công!";
            return RedirectToAction("List");
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(UpdateUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                foreach (var error in errors)
                {
                    Console.WriteLine(error); // Ghi log lỗi
                }
                ViewBag.Roles = await _userService.GetRolesAsync();
                var users = await _userService.GetAllUsersAsync();
                return View("List", users);
            }

            var existingUser = await _userManager.FindByIdAsync(model.Id);
            if (existingUser == null)
            {
                ModelState.AddModelError("", "User not found.");
                ViewBag.Roles = await _userService.GetRolesAsync();
                var users = await _userService.GetAllUsersAsync();
                return View("List", users);
            }

            // Cập nhật thông tin người dùng
            existingUser.UserName = model.UserName;
            existingUser.FullName = model.FullName;
            existingUser.Email = model.Email;
            var roles = await _userManager.GetRolesAsync(existingUser);
            await _userManager.RemoveFromRolesAsync(existingUser, roles.ToArray());
            var role = await _roleManager.FindByIdAsync(model.RoleId);
            if (role == null)
            {
                ModelState.AddModelError("", "Selected role does not exist.");
                ViewBag.Roles = await _userService.GetRolesAsync();
                var users = await _userService.GetAllUsersAsync();
                TempData["MessageUpdateUserFail"] = "Chỉnh sửa người dùng thành công kiểm tra lại role";
                return View("List", users);
            }

            await _userManager.AddToRoleAsync(existingUser, role.Name);


            var result = await _userManager.UpdateAsync(existingUser);
            if (!result.Succeeded)
            {
                TempData["MessageUpdateUserFail"] = "Chỉnh sửa người dùng thành công kiểm tra lại thông tin!";
                ModelState.AddModelError("", "Failed to update user.");
                ViewBag.Roles = await _userService.GetRolesAsync();
                var users = await _userService.GetAllUsersAsync();
                return View("List", users);
            }
            TempData["MessageUpdateUserSuccess"] = "Chỉnh sửa người dùng thành công!";
            return RedirectToAction("List");
        }


    }
}
