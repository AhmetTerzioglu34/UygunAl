using System.ComponentModel.DataAnnotations;

namespace Project.COREMVC.Models.PureVms.ResetPassword
{
    public class ResetPasswordPureVm
    {
        [Required(ErrorMessage = "{0} zorunludur")]
        [Display(Name = "Yeni şifre alanı")]
        [MinLength(3, ErrorMessage = "Minimum {1} karakter girilmesi lazım")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "{0} zorunludur")]
        [Display(Name = "Yeni şifre tekrarı")]
        [Compare("NewPassword", ErrorMessage = "Şifreler uyuşmuyor")]
        public string ConfirmPassword { get; set; }
    }
}
