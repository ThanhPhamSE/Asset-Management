using Asset_Management.Models;
using Asset_Management.Services.IServices;
using Asset_Management.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Asset_Management.Controllers
{
    public class RoleController : Controller
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        public async Task<IActionResult> List()
        {
            var roles = await _roleService.GetAllRolesAsync();

            // Chuyển đổi từ Roles sang RoleViewModel
            var roleViewModels = roles.Select(r => new RoleViewModel
            {
                Id = r.Id,
                Name = r.Name
            }).ToList();

            return View(roleViewModels); // Trả về danh sách RoleViewModel
        }

        [HttpPost]
        public async Task<IActionResult> Create(RoleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Invalid input. Please check your data.";
                return RedirectToAction("List");
            }

            // Kiểm tra xem role đã tồn tại chưa
            var existingRole = await _roleService.GetRoleByNameAsync(model.Name);
            if (existingRole != null)
            {
                TempData["ErrorMessage"] = "Role already exists!";
                return RedirectToAction("List");
            }

            var role = new Roles
            {
                Id = Guid.NewGuid().ToString(),
                Name = model.Name,
                NormalizedName = model.Name.ToUpper()
            };

            await _roleService.AddRoleAsync(role);
            TempData["SuccessMessage"] = "Role added successfully!";
            return RedirectToAction("List");
        }




        [HttpPost]
        public async Task<IActionResult> Edit(RoleViewModel model)
        {
            Console.WriteLine($"Received Id: {model.Id}, Name: {model.Name}");

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                Console.WriteLine("Validation Errors: " + string.Join("; ", errors));
                TempData["ErrorMessage"] = string.Join("; ", errors);
                return RedirectToAction("List");
            }

            var role = await _roleService.GetRoleByIdAsync(model.Id);
            if (role == null)
            {
                TempData["ErrorMessage"] = "Role not found.";
                return RedirectToAction("List");
            }

            role.Name = model.Name;
            role.NormalizedName = model.Name.ToUpper();

            await _roleService.UpdateRoleAsync(role);

            TempData["SuccessMessage"] = "Role updated successfully!";
            return RedirectToAction("List");
        }




        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                TempData["ErrorMessage"] = "Invalid role ID.";
                return RedirectToAction("List");
            }

            await _roleService.DeleteRoleAsync(id);
            TempData["SuccessMessage"] = "Role deleted successfully!";
            return RedirectToAction("List");
        }
    }
}
