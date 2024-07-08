using Project.COREMVC.Models.PureVms.Orders;
using Project.COREMVC.Models.PureVms.OuterModelsForBank;

namespace Project.COREMVC.Models.PageVms.OrderPageVm
{
    public class OrderRequestPageVm
    {
        public OrderPureVm  OrderPureVm { get; set; }
        public PaymentRequestPureVm PaymentRequestPureVm { get; set; }
    }
}
