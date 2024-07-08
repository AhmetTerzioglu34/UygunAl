using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Project.BLL.Managers.Abstracts;
using Project.BLL.Managers.Concretes;
using Project.COREMVC.Areas.Admin.Models.PageVms.EntityAttribute;
using Project.COREMVC.Areas.Admin.Models.PureVms.EntityAttribute;
using Project.ENTITIES.Models;
using System;

namespace Project.COREMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AttributeController : Controller
    {
        readonly IEntityAttributeManager _entityAttributeManager;
        readonly IEntityAttributeProductManager _entityAttributeProductManager;
        readonly IProductManager _productManager;

        public AttributeController(IEntityAttributeManager entityAttributeManager, IEntityAttributeProductManager entityAttributeProductManager, IProductManager productManager)
        {
            _entityAttributeManager = entityAttributeManager;
            _entityAttributeProductManager = entityAttributeProductManager;
            _productManager = productManager;
        }


        public async Task<IActionResult> GetAttributeProduct(int id)
        {


            List<EntityAttributeProduct> eAProduct = _entityAttributeProductManager.Where(x => x.ProductID == id).ToList();

            List<EntityAttribute> eAttributes = _entityAttributeManager.GetAll();


            List<EntityAttribute> newEAttributess = eAttributes.Join(eAProduct,
           eAttr => eAttr.ID,  // eAttributes'taki ID
           eProd => eProd.EntityAttributeID,  // eAProducts'taki EntityAttributeID
           (eAttr, eProd) => eAttr)  // Eşleşen eAttributes nesnelerini al
                                     .ToList();

            List<EntityAttributePureVm> entityAttributePureVms = newEAttributess.Select(x => new EntityAttributePureVm
            {
                AttributeName = x.AttributeName,
                ID = x.ID
            }).ToList();

            List<EntityAttributeProductPureVm> entityAttributeProductPureVms = eAProduct.Select(x => new EntityAttributeProductPureVm
            {
                Value = x.Value,
                ProductID = x.ProductID,
                EntityAttributeID = x.EntityAttributeID
            }).ToList();


            EntityAttributePageVm pageVm = new EntityAttributePageVm();
            pageVm.EntityAttributeProductPureVms = entityAttributeProductPureVms;
            pageVm.EntityAttributePureVms = entityAttributePureVms;

            if ( pageVm.EntityAttributePureVms.Count ==0 )
            {
                
                return RedirectToAction("AttributeProduct", new { id });
            }


            return View(pageVm);




        }


        public async Task<IActionResult> AttributeProduct(int id)
        {
            EntityAttributeCreatePureVm pureVm = new()
            {
                ProductID = id,

            };
            EntityAttributeCreatePageVm pageVm = new EntityAttributeCreatePageVm();
            pageVm.EntityAttributeCreatePureVm = pureVm;
            return View(pageVm);
        }


        [HttpPost]
        public async Task<IActionResult> AttributeProduct(EntityAttributeCreatePageVm pageVm)
        {

            EntityAttribute entityAttribute = new();

            if ( await _entityAttributeManager.AnyAsync(x=> x.AttributeName == pageVm.EntityAttributeCreatePureVm.AttributeName))
            {

                 entityAttribute =  _entityAttributeManager.FirstOrDefault(x => x.AttributeName == pageVm.EntityAttributeCreatePureVm.AttributeName);
               
            }
            else
            {

                entityAttribute.AttributeName = pageVm.EntityAttributeCreatePureVm.AttributeName;
                
                await _entityAttributeManager.AddAsync(entityAttribute);
               
            }
            if (await _entityAttributeProductManager.AnyAsync(x => x.ProductID == pageVm.EntityAttributeCreatePureVm.ProductID && x.EntityAttributeID == entityAttribute.ID))
            {
                TempData["message"] = $"{entityAttribute.AttributeName} isimli özellik bu üründe bulunmakadır ";
            }
            else
            {
                EntityAttributeProduct entityAttributeProduct = new()
                {
                    EntityAttributeID = entityAttribute.ID,
                    Value = pageVm.EntityAttributeCreatePureVm.Value,
                    ProductID = pageVm.EntityAttributeCreatePureVm.ProductID
                };
                await _entityAttributeProductManager.AddAsync(entityAttributeProduct);
                TempData["message"] = $"{entityAttributeProduct.ProductID} ID'li ürüne özellik eklenmiştir";
            }
           




            return View(pageVm);
        }

        public async Task<IActionResult> DestroyAttribute(int productId, int attributeId)
        {

            EntityAttributeProduct? entityAttributeProduct = _entityAttributeProductManager.Where(e => e.ProductID == productId && e.EntityAttributeID == attributeId).SingleOrDefault();

            if (entityAttributeProduct != null)
            {
                _entityAttributeProductManager.Delete(entityAttributeProduct);
                _entityAttributeProductManager.Destroy(entityAttributeProduct);
            }

            // EntityAttribute'yu bulun ve silin
          
                TempData["message"] = $"{entityAttributeProduct.ID} ID'li özellik başarıyla yok edildi";
            
           

            return RedirectToAction("GetProducts", "Product");

        }

        public async Task<IActionResult> UpdateAttribute(int productId, int attributeId)
        {
            EntityAttributeProduct? entityAttributeProduct = _entityAttributeProductManager.Where(e => e.ProductID == productId && e.EntityAttributeID == attributeId).SingleOrDefault();

            UpdateAttributeProductPureVm aPPureVm = new UpdateAttributeProductPureVm()
            {
                EntityAttributeID = entityAttributeProduct.EntityAttributeID,
                ProductID = productId,
                Value = entityAttributeProduct.Value
            };

            EntityAttribute entityAttribute = await _entityAttributeManager.FindAsync(attributeId);

            UpdateAttributePureVm aPureVm = new()
            {
                AttributeID = entityAttribute.ID,
                AttributeName = entityAttribute.AttributeName
            };


            UpdateAttributeProductPageVm updateAttributeProductPageVm = new()
            {
                UpdateAttributeProductPureVm = aPPureVm,
                UpdateAttributePureVm = aPureVm
            };


            return View(updateAttributeProductPageVm);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateAttribute(UpdateAttributeProductPageVm pageVm)
        {

            // EntityAttribute güncelleme
            EntityAttribute entityAttribute = await _entityAttributeManager.FindAsync(pageVm.UpdateAttributePureVm.AttributeID);

            entityAttribute.AttributeName = pageVm.UpdateAttributePureVm.AttributeName;
            await _entityAttributeManager.UpdateAsync(entityAttribute);

            EntityAttributeProduct entityAttributeProduct = new EntityAttributeProduct();
            entityAttributeProduct.EntityAttributeID = pageVm.UpdateAttributePureVm.AttributeID;
            entityAttributeProduct.ProductID = pageVm.UpdateAttributeProductPureVm.ProductID;
            entityAttributeProduct.Value = pageVm.UpdateAttributeProductPureVm.Value;

            EntityAttributeProduct originalAttribute = _entityAttributeProductManager.Where(x => x.ProductID == entityAttributeProduct.ProductID && x.EntityAttributeID == entityAttributeProduct.EntityAttributeID).FirstOrDefault();
            _entityAttributeProductManager.UpdateForJunction(entityAttributeProduct , originalAttribute);

            TempData["message"] = $"{originalAttribute.ProductID} ID'li ürünün {originalAttribute.EntityAttributeID } ID'li özelliği değiştirilmiştir";



           


            return RedirectToAction("GetProducts", "Product");
        }
    }
}
