using Project.COREMVC.Models.PureVms.Shoppings;
using X.PagedList;

namespace Project.COREMVC.Models.PageVms.Shopping
{
    public class ShoppingPageVm
    {
        public IPagedList<ProductForShoppingPureVm> ProductForShoppingPureVms { get; set; }
        public List<CategoryForShoppingPureVm> CategoryForShoppingPureVms { get; set; }
    }
}
