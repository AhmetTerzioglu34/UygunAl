using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.BLL.Managers.Abstracts;
using Project.CONF.Configutaion;
using Project.COREMVC.Areas.Admin.Models.PageVms.Category;
using Project.COREMVC.Areas.Admin.Models.PageVms.EntityAttribute;
using Project.COREMVC.Areas.Admin.Models.PageVms.Product;
using Project.COREMVC.Areas.Admin.Models.PureVms.Category;
using Project.COREMVC.Areas.Admin.Models.PureVms.EntityAttribute;
using Project.COREMVC.Areas.Admin.Models.PureVms.Product;
using Project.COREMVC.Models.PageVms.Product;
using Project.COREMVC.Models.PureVms.Products;
using Project.COREMVC.Models.ShoppingTools;
using Project.ENTITIES.Models;


namespace Project.COREMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin")]
    public class ProductController : Controller
    {
        readonly IProductManager _productManager;
        readonly ICategoryManager _categoryManager;
       

        public ProductController(IProductManager productManager, ICategoryManager categoryManager)
        {
            _productManager = productManager;
            _categoryManager = categoryManager;
            
        }

        public IActionResult GetProducts()
        {
            List<Product> products = _productManager.GetActives();
          

            List<ProductPureVm> pureVms = products.Select(pureVms => new ProductPureVm
            {
                ID = pureVms.ID,
                ProductName = pureVms.ProductName,
                UnitPrice = pureVms.UnitPrice,
                UnitsInStock = pureVms.UnitsInStock,
                Status = pureVms.Status,
                ImagePath = pureVms.ImagePath,
                CategoryName = pureVms.Category.CategoryName,


                

            }).ToList();
            

            ProductPageVm pageVm = new ProductPageVm();
            pageVm.ProductPureVms = pureVms;

            return View(pageVm);

            
        }
        
        public IActionResult CreateProduct()
        {
          

            List<Category> categories = _categoryManager.GetActives();

            List<CategoryProductPureVm> categoryProductPureVm = categories.Select(c => new CategoryProductPureVm
            {
                ID = c.ID,
                CategoryName = c.CategoryName
               
            }).ToList();

            CreateProductPageVm cppVm = new CreateProductPageVm()
            {
                CategoryProductPureVms = categoryProductPureVm
            };

            return View(cppVm);

        }
        [HttpPost]
        public IActionResult CreateProduct(CreateProductPageVm createProductPageVm, IFormFile formFile)
        {


            
            if (formFile != null )
            {
                Guid uniqueName = Guid.NewGuid();
                string extension = Path.GetExtension(formFile.FileName); //dosyanın uzantısını ele gecirdik...
                createProductPageVm.CreateProductPureVm.ImagePath = $"/images/{uniqueName}{extension}";
                //Bulunduğunuz konuma göre mevcut konumu alır 
                string path = $"{Directory.GetCurrentDirectory()}/wwwroot{createProductPageVm.CreateProductPureVm.ImagePath}";
                FileStream stream = new(path, FileMode.Create);
                formFile.CopyTo(stream);
            }
            else
            {
                createProductPageVm.CreateProductPureVm.ImagePath = "/OuterImages/DenemeFoto.png";
            }

           
          

            Product p = new Product()
            {
                ProductName = createProductPageVm.CreateProductPureVm.ProductName,
                ImagePath = createProductPageVm.CreateProductPureVm.ImagePath,
                UnitPrice = createProductPageVm.CreateProductPureVm.UnitPrice,
                CategoryID = createProductPageVm.CreateProductPureVm.CategoryID,
                UnitsInStock = createProductPageVm.CreateProductPureVm.UnitsInStock,
                
            };

            _productManager.Add(p);

            TempData["message"] = $"{p.ProductName} isimli ürün eklenmiştir";
            return RedirectToAction("GetProducts");


        }
           

        


        public async Task<IActionResult> DeleteProduct(int id)
        {
            _productManager.Delete(await _productManager.FindAsync(id));
            return RedirectToAction("GetProducts");
        }

        public async  Task<IActionResult> DestroyProduct(int id)
        {
            TempData["message"] = _productManager.Destroy(await _productManager.FindAsync(id));
            return RedirectToAction("GetProducts");
        }

        public async Task<IActionResult> UpdateProduct(int id)
        {

            Product p = await _productManager.FindAsync(id);

            

            UpdateProductPureVm updateProductPureVm = new UpdateProductPureVm()
            {
                ID = p.ID,
                ProductName = p.ProductName,
                ImagePath = p.ImagePath,
                UnitPrice = p.UnitPrice,
                UnitsInStock= p.UnitsInStock,
                CategoryID = p.Category.ID
                
               
            };

            List<Category> categories = _categoryManager.GetActives();

            List<CategoryProductPureVm> categoryProductPureVm = categories.Select(c => new CategoryProductPureVm
            {
                ID = c.ID,
                CategoryName = c.CategoryName
                

            }).ToList();

            UpdateProductPageVm upPageVm = new UpdateProductPageVm()
            {
                CategoryProductPureVms = categoryProductPureVm,
                UpdateProductPureVm = updateProductPureVm
                
            };

            return View(upPageVm);

        }

        [HttpPost]
        public async Task<IActionResult> UpdateProduct(UpdateProductPageVm updateProductPageVm, IFormFile formFile)
        {
            if (formFile != null)
            {
                Guid uniqueName = Guid.NewGuid();
                string extension = Path.GetExtension(formFile.FileName); //dosyanın uzantısını ele gecirdik...
                updateProductPageVm.UpdateProductPureVm.ImagePath = $"/images/{uniqueName}{extension}";
                //Bulunduğunuz konuma göre mevcut konumu alır 
                string path = $"{Directory.GetCurrentDirectory()}/wwwroot{updateProductPageVm.UpdateProductPureVm.ImagePath}";
                FileStream stream = new(path, FileMode.Create);
                formFile.CopyTo(stream);
            }
            
            Product p = new()
            {
                    ID = updateProductPageVm.UpdateProductPureVm.ID,
                    ProductName = updateProductPageVm.UpdateProductPureVm.ProductName,
                    ImagePath = updateProductPageVm.UpdateProductPureVm.ImagePath,
                    UnitPrice = updateProductPageVm.UpdateProductPureVm.UnitPrice,
                    CategoryID = updateProductPageVm.UpdateProductPureVm.CategoryID,
                    UnitsInStock = updateProductPageVm.UpdateProductPureVm.UnitsInStock

            };
                TempData["message"] = $"{p.ProductName} isimli veri güncellenmiştir";
                await _productManager.UpdateAsync(p);
                return RedirectToAction("GetProducts");
            

        }



       
       




    }

}
