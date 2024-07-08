using System.ComponentModel.DataAnnotations;

namespace Project.COREMVC.Models.PureVms.OuterModelsForBank
{
    public class PaymentRequestPureVm
    {
        [Required(ErrorMessage = "{0} zorunludur")]
        [Display(Name = "Kart numarası")]
        [MaxLength(19, ErrorMessage = "{0} en fazla {1} karakter alabilir")]
        [MinLength(19, ErrorMessage = "{0} en az {1} karakter alabilir")]
        public string CardNumber { get; set; }



        [Required(ErrorMessage = "{0} zorunludur")]
        [Display(Name = "Kart kullanıcı ismi")]
        public string CardUserName { get; set; }




        [Required(ErrorMessage = "{0} zorunludur")]
        [Display(Name = "CVC")]
        [RegularExpression(@"^\d+$", ErrorMessage = "CVC yalnızca rakam içermelidir")]
        [MaxLength(3, ErrorMessage = "{0} en fazla {1} karakter alabilir")]
        [MinLength(3, ErrorMessage = "{0} en az {1} karakter alabilir")]
        public string CVC { get; set; }


        [Required(ErrorMessage = "{0} zorunludur")]
        [Display(Name = "Yıl")]
        [Range(1900, 2100, ErrorMessage = "{0} 1900 ile 2100 arasında bir değer olmalıdır")]
        public int ExpiryYear { get; set; }


        [Required(ErrorMessage = "{0} zorunludur")]
        [Display(Name = "Ay")]
        [Range(1, 12, ErrorMessage = "{0} 1 ile 12 arasında bir değer olmalıdır")]
        public int ExpiryMonth { get; set; }


        public decimal ShoppingPrice { get; set; }

    }
}
