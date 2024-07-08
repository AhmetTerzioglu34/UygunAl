using Project.COREMVC.Models.PureVms.Attribute;
using Project.COREMVC.Models.PureVms.Products;

namespace Project.COREMVC.Models.PageVms.Product
{
    public class ProductDetailPageVm
    {
        public ProductDetailPureVm ProductDetailPureVm { get; set; }
        public List<AttributePureVm> AttributePureVms { get; set; }

    }
}
