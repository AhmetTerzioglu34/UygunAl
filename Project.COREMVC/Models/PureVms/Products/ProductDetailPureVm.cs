using System.ComponentModel.DataAnnotations;

namespace Project.COREMVC.Models.PureVms.Products
{
    public class ProductDetailPureVm
    {
        public int ID { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
      
        public string ImagePath { get; set; }
       
        public string CategoryName { get; set; }

        //public string AttributeName { get; set; }

    }
}
