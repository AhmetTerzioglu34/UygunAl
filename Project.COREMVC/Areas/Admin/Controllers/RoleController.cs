using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.COREMVC.Areas.Admin.Models.PageVms.AppRole;
using Project.COREMVC.Areas.Admin.Models.PureVms.AppRole;
using Project.COREMVC.Areas.Admin.Models.PureVms.AppUser;
using Project.ENTITIES.Models;

namespace Project.COREMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        readonly RoleManager<AppRole> _roleManager;

        public RoleController(RoleManager<AppRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<IActionResult> GetRoles()
        {
          List<AppRole> appRoles =  await _roleManager.Roles.ToListAsync();

            List<GetRolesPureVm> getAppRoles = appRoles.Select(x => new GetRolesPureVm
            {
                ID = x.Id,
                RoleName = x.Name,
                CreatedDate = x.CreatedDate
            }).ToList();

            GetAppRolePageVm pageVm = new GetAppRolePageVm();
            pageVm.GetRolesPureVms = getAppRoles;

            return View(pageVm);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateRoleRequestModel  createRoleRequestModel)
        {
            if (ModelState.IsValid)
            {
                IdentityResult result = await _roleManager.CreateAsync(new()
                {
                    Name = createRoleRequestModel.RoleName
                });

                if (result.Succeeded)
                {
                    return RedirectToAction("GetRoles");
                }

                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(createRoleRequestModel);
           
        }
     
        public async Task<IActionResult> DestroyRole(int id)
        {

            AppRole role = await _roleManager.FindByIdAsync(id.ToString());
            await _roleManager.DeleteAsync(role);
            return RedirectToAction("GetRoles");
        }
       
    }
}
