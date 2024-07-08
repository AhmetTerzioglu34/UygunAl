using Project.COREMVC.Areas.Admin.Models.PureVms.Category;
using Project.COREMVC.Areas.Admin.Models.PureVms.Product;

namespace Project.COREMVC.Areas.Admin.Models.PageVms.Product
{
    public class UpdateProductPageVm
    {
        public List<CategoryProductPureVm> CategoryProductPureVms { get; set; }
        public UpdateProductPureVm UpdateProductPureVm { get; set; }
    }
}
