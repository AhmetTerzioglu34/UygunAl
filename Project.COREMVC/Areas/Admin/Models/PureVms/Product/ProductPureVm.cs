using Project.COREMVC.Areas.Admin.Models.PureVms.Category;
using Project.ENTITIES.Enums;
using System.ComponentModel.DataAnnotations;

namespace Project.COREMVC.Areas.Admin.Models.PureVms.Product
{
    public class ProductPureVm
    {
        public int ID { get; set; }

        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public int UnitsInStock { get; set; }
        public string ImagePath { get; set; }
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string AttributeName { get; set; }
        public string Value { get; set; }

        public DataStatus Status { get; set; }
        
    }
}
