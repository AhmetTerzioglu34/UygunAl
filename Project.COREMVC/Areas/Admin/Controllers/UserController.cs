using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.COREMVC.Areas.Admin.Models.PageVms.AppRole;
using Project.COREMVC.Areas.Admin.Models.PageVms.AppUser;
using Project.COREMVC.Areas.Admin.Models.PureVms.AppRole;
using Project.COREMVC.Areas.Admin.Models.PureVms.AppUser;
using Project.ENTITIES.Models;

namespace Project.COREMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        readonly UserManager<AppUser> _userManager;
        readonly RoleManager<AppRole> _roleManager;

        public UserController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {


            List<AppUser> nonAdminUsers = _userManager.Users.Where(x => !x.UserRoles.Any(x => x.Role.Name == "Admin")).ToList();

            List<AppUserPureVm> appUsers = nonAdminUsers.Select(x => new AppUserPureVm
            {
                UserName = x.UserName,
                Email = x.Email,
                ID = x.Id,
            }).ToList();

            AppUserPageVm pageVm = new()
            {
                AppUserPureVms = appUsers
            };

            return View(pageVm);
        }


        public async Task<IActionResult> AssignRole(int id)
        {
            AppUser appUser = await _userManager.Users.SingleOrDefaultAsync(x => x.Id == id);

            IList<string> userRoles = await _userManager.GetRolesAsync(appUser); // Elimize gecen kullanıcının rollerini verir

            TempData["message"] = $"Rol değiştirmek istediğiniz kullanıcının adı {appUser.UserName}";

            List<AppRole> allRoles = _roleManager.Roles.ToList(); //bütün roller

            List<AppRoleResponseModel> roles = new();


            foreach (AppRole role in allRoles)
            {
                roles.Add(new()
                {
                    RoleID = role.Id,
                    RoleName = role.Name,
                    Checked = userRoles.Contains(role.Name)
                });
            }

            AppRolePageVM aRPageVm = new()
            {
                UserID = id,
                Roles = roles
            };

            return View(aRPageVm);
        }
        [HttpPost]
        public async Task<IActionResult> AssignRole(AppRolePageVM model)
        {
            AppUser appUser = await _userManager.Users.SingleOrDefaultAsync(x => x.Id == model.UserID);
            IList<string> userRoles = await _userManager.GetRolesAsync(appUser);

            

            foreach (AppRoleResponseModel role in model.Roles)
            {
                
                if (role.Checked && !userRoles.Contains(role.RoleName))
                {
                    await _userManager.AddToRoleAsync(appUser, role.RoleName);
                   
                }

                else if (!role.Checked && userRoles.Contains(role.RoleName))
                {
                    await _userManager.RemoveFromRoleAsync(appUser, role.RoleName);
                   
                }
                
            }
            

            return RedirectToAction("Index");
        }

    }
}
