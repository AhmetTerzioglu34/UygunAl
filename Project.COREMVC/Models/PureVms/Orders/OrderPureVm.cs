using Project.ENTITIES.Models;
using System.ComponentModel.DataAnnotations;

namespace Project.COREMVC.Models.PureVms.Orders
{
    public class OrderPureVm
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "{0} zorunludur")]
        [Display(Name = "Gönderi Adresi")]
        [MinLength(8, ErrorMessage = "{0} en az {1} karakter alabilir")]
        public string ShippingAddress { get; set; }


        [Required(ErrorMessage = "{0} zorunludur")]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Email formatında giriniz")]
        public string? Email { get; set; }



        [Required(ErrorMessage = "{0} zorunludur")]
        [Display(Name = "İsim Soyisim")]
        [MinLength(6, ErrorMessage = "{0} en az {1} karakter alabilir")]
        public string NameDescription { get; set; }
        public int? AppUserID { get; set; }
        public decimal PriceOfOrder { get; set; }
        

        
    }
}
