using Project.COREMVC.Models.ShoppingTools;

namespace Project.COREMVC.Models.PageVms.Shopping
{
    public class CartPageVm
    {
        public Cart Cart { get; set; }
        public List<CartItem> CartItems { get; set; }
    }
}
