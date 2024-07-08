using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.BLL.Managers.Abstracts;
using Project.COREMVC.Areas.Admin.Models.PageVms.Category;
using Project.COREMVC.Areas.Admin.Models.PureVms.Category;
using Project.ENTITIES.Models;
using System.Collections.Generic;

namespace Project.COREMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {


        readonly ICategoryManager _categoryManager;

        public CategoryController(ICategoryManager categoryManager)
        {
            _categoryManager = categoryManager;
        }

        public IActionResult GetCategories()
        {
            List<Category> categories = _categoryManager.GetAll();

            List<CategoryPureVm> pureVms = categories.Select(pureVms => new CategoryPureVm
            {
                ID = pureVms.ID,
                CategoryName = pureVms.CategoryName,
                Description = pureVms.Description,
                Status = pureVms.Status
            }).ToList();

            CategoryPageVm pageVm = new CategoryPageVm();
            pageVm.CategoryPureVms = pureVms;
        
            return View(pageVm);

        }

        public IActionResult CreateCategory()
        {


            return View();
        }
        [HttpPost]
        public async Task <IActionResult> CreateCategory(CreateCategoryPageVm pageVm)
        {
            if (ModelState.IsValid)
            {

                Category c = new()
                {
                    CategoryName = pageVm.CreateCategoryRequestModel.CategoryName,
                    Description = pageVm.CreateCategoryRequestModel.Description

                };
                await _categoryManager.AddAsync(c);

                TempData["message"] = $"{c.CategoryName} isimli kategori başarılı bir şekilde eklenmiştir";
                return RedirectToAction("GetCategories");
              
            }

            return View(pageVm);

        }
        public async  Task<IActionResult> DeleteCategory(int id)
        {


            _categoryManager.Delete(await _categoryManager.FindAsync(id));
                return RedirectToAction("GetCategories");
            
        }

        public async Task<IActionResult> DestroyCategory(int id)
        {
            
            TempData["message"] = _categoryManager.Destroy(await _categoryManager.FindAsync(id));
            return RedirectToAction("GetCategories");
        }

        public  async Task< IActionResult> UpdateCategory(int id)
        {
            Category c = await _categoryManager.FindAsync(id);

            UpdateCategoryPureVm ucPureVm = new()
            {
                CategoryName = c.CategoryName,
                Description = c.Description,
                ID = c.ID
            };

            UpdateCategoryPageVm ucPvm = new UpdateCategoryPageVm()
            {
                UpdateCategoryPureVm = ucPureVm,
            };



            return View(ucPvm);

        }
        [HttpPost]
        public async Task<IActionResult> UpdateCategory(UpdateCategoryPageVm pageVm)
        {
            if (ModelState.IsValid)
            {
                Category c = new()
                {
                    ID = pageVm.UpdateCategoryPureVm.ID,
                    CategoryName = pageVm.UpdateCategoryPureVm.CategoryName,
                    Description = pageVm.UpdateCategoryPureVm.Description

                };
                TempData["message"] = $"{c.CategoryName} isimli veri güncellenmiştir";
                await _categoryManager.UpdateAsync(c);
                return RedirectToAction("GetCategories");
            }
            return View(pageVm);
             
            
           
        }

    }
}
