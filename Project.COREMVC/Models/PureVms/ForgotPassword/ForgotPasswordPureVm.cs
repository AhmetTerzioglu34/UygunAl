using System.ComponentModel.DataAnnotations;

namespace Project.COREMVC.Models.PureVms.ForgotPassword
{
    public class ForgotPasswordPureVm
    {
        [Required(ErrorMessage = "{0} zorunludur")]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Email formatında giriniz")]
        public string Email { get; set; }
    }
}
