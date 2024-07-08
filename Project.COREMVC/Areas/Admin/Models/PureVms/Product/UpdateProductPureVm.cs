using Project.ENTITIES.Enums;
using System.ComponentModel.DataAnnotations;

namespace Project.COREMVC.Areas.Admin.Models.PureVms.Product
{
    public class UpdateProductPureVm
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "{0} zorunludur")]
        [Display(Name = "Ürün ismi")]
        [MaxLength(40, ErrorMessage = "{0} en fazla {1} karakter alabilir")]
        [MinLength(2, ErrorMessage = "{0} en az {1} karakter alabilir")]
        public string ProductName { get; set; }



        [Required(ErrorMessage = "{0} zorunludur")]
        [Display(Name = "Fiyat bilgisi")]
        public decimal UnitPrice { get; set; }




        [Required(ErrorMessage = "{0} zorunludur")]
        [Display(Name = "Stok bilgisi")]
        public int UnitsInStock { get; set; }



        public string ImagePath { get; set; }

        public int CategoryID { get; set; }


        [Required(ErrorMessage = "{0} zorunludur")]
        [Display(Name = "Kategori ismi")]
        public string CategoryName { get; set; }


        public string AttributeName { get; set; }
    }
}
