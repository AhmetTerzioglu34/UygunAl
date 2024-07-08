using System.ComponentModel.DataAnnotations;

namespace Project.COREMVC.Areas.Admin.Models.PureVms.AppRole
{
    public class CreateRoleRequestModel
    {
        [Required(ErrorMessage = "{0} zorunludur")]
        [Display(Name = "Rol ismi")]
        [MaxLength(20, ErrorMessage = "{0} en fazla {1} karakter alabilir")]
        [MinLength(2, ErrorMessage = "{0} en az {1} karakter alabilir")]
        public string RoleName { get; set; }
    }
}
