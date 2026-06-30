using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NotikaIdentityEmail.Entities;
using NotikaIdentityEmail.Models;

namespace NotikaIdentityEmail.Controllers;

public class RoleController(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager) : Controller
{
    public async ValueTask<IActionResult> RoleList()
    {
        ViewBag.Title = "Rol Listesi";
        ViewBag.Description = "Rol işlemlerini bu sayfa üzerinden gerçekleştirebilirsiniz.";

        var roles = await roleManager.Roles.ToListAsync();
        return View(roles);
    }

    [HttpGet]
    public IActionResult CreateRole()
    {
        ViewBag.Title = "Rol Ekle";
        ViewBag.Description = "Rol ekleme işlemini bu sayfa üzerinden gerçekleştirebilirsiniz.";

        return View();
    }

    [HttpPost]
    public async ValueTask<IActionResult> CreateRole(CreateRoleViewModel model)
    {
        IdentityRole role = new IdentityRole()
        {
            Name = model.Role
        };

        await roleManager.CreateAsync(role);

        return RedirectToAction(nameof(RoleList));
    }

    public async ValueTask<IActionResult> DeleteRole(string id)
    {
        var role = await roleManager.FindByIdAsync(id);
        await roleManager.DeleteAsync(role);

        return RedirectToAction(nameof(RoleList));
    }

    [HttpGet]
    public async ValueTask<IActionResult> UpdateRole(string id)
    {
        ViewBag.Title = "Rol Güncelle";
        ViewBag.Description = "Rol güncelleme işlemini bu sayfa üzerinden gerçekleştirebilirsiniz.";

        var role = await roleManager.FindByIdAsync(id);
        UpdateRoleViewModel model = new UpdateRoleViewModel()
        {
            Id = role.Id,
            Role = role.Name
        };

        return View(model);
    }

    [HttpPost]
    public async ValueTask<IActionResult> UpdateRole(UpdateRoleViewModel model)
    {
        var role = await roleManager.FindByIdAsync(model.Id);
        role.Name = model.Role;

        await roleManager.UpdateAsync(role);

        return RedirectToAction(nameof(RoleList));
    }

    public async ValueTask<IActionResult> UserList()
    {
        ViewBag.Title = "Kullanıcı Listesi";
        ViewBag.Description = "Kullanıcı işlemlerini bu sayfa üzerinden gerçekleştirebilirsiniz.";

        var users = await userManager.Users.ToListAsync();
        return View(users);
    }

    [HttpGet]
    public async ValueTask<IActionResult> AssignRole(string id)
    {
        ViewBag.Title = "Rol Atama";
        ViewBag.Description = "Kullanıcı rol işlemlerini bu sayfa üzerinden gerçekleştirebilirsiniz.";

        var user = await userManager.FindByIdAsync(id);
        TempData["userId"] = user.Id;

        var roles = await roleManager.Roles.ToListAsync();
        var userRoles = await userManager.GetRolesAsync(user);

        List<RoleAssignViewModel> roleAssignViewModels = new List<RoleAssignViewModel>();
        foreach (var item in roles)
        {
            RoleAssignViewModel model = new RoleAssignViewModel();
            model.Id = item.Id;
            model.Role = item.Name;
            model.Exist = userRoles.Contains(item.Name);

            roleAssignViewModels.Add(model);
        }

        return View(roleAssignViewModels);
    }

    [HttpPost]
    public async ValueTask<IActionResult> AssignRole(List<RoleAssignViewModel> model)
    {
        var userId = TempData["userId"].ToString();
        var user = await userManager.FindByIdAsync(userId);

        foreach (var item in model)
        {
            if (item.Exist)
            {
                await userManager.AddToRoleAsync(user, item.Role);
            }
            else
            {
                await userManager.RemoveFromRoleAsync(user, item.Role);
            }
        }

        return RedirectToAction(nameof(UserList));
    }
}