using System.ComponentModel.DataAnnotations;

namespace Project.COREMVC.Models.PureVms.Shoppings
{
    public class ProductForShoppingPureVm
    {
        public int ID { get; set; }

        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public int UnitsInStock { get; set; }
        public string ImagePath { get; set; }
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
    }
}
