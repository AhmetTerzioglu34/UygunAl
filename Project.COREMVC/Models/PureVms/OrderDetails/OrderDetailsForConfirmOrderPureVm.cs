namespace Project.COREMVC.Models.PureVms.OrderDetails
{
    public class OrderDetailsForConfirmOrderPureVm
    {
        public int OrderID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
