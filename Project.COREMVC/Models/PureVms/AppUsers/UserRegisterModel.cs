using System.ComponentModel.DataAnnotations;

namespace Project.COREMVC.Models.PureVms.AppUsers
{

    public class UserRegisterModel
    {
        [Required(ErrorMessage = "{0} zorunludur")]
        [Display(Name = "Kullanıcı ismi")]
        [MaxLength(20, ErrorMessage = "{0} en fazla {1} karakter alabilir")]
        [MinLength(3, ErrorMessage = "{0} en az {1} karakter alabilir")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "{0} zorunludur")]
        [Display(Name = "Şifre")]
        [MaxLength(16, ErrorMessage = "{0} en fazla {1} karakter alabilir")]
        [MinLength(8, ErrorMessage = "{0} en az {1} karakter alabilir")]
        public string Password { get; set; }

        [Required(ErrorMessage = "{0} zorunludur")]
        [Display(Name = "Şifre tekrarı")]
        [Compare("Password" , ErrorMessage ="Şifreler aynı değil!")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "{0} zorunludur")]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Email formatında giriniz")]
        
        public string Email { get; set; }
    }
}
