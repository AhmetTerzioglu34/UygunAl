using Project.COREMVC.Areas.Admin.Models.PureVms.Category;
using Project.COREMVC.Areas.Admin.Models.PureVms.EntityAttribute;
using Project.COREMVC.Areas.Admin.Models.PureVms.Product;
using Project.ENTITIES.Models;

namespace Project.COREMVC.Areas.Admin.Models.PageVms.Product
{
    public class CreateProductPageVm
    {
        public List<CategoryProductPureVm> CategoryProductPureVms { get; set; }
        public CreateProductPureVm CreateProductPureVm { get; set; }
        
    }
}
