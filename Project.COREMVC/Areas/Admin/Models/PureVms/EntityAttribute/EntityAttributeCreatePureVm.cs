using System.ComponentModel.DataAnnotations;

namespace Project.COREMVC.Areas.Admin.Models.PureVms.EntityAttribute
{
    public class EntityAttributeCreatePureVm
    {
        public int ProductID { get; set; }

        [Required(ErrorMessage = "{0} zorunludur")]
        [Display(Name = "Özellik ismi")]
        public string AttributeName { get; set; }

        [Required(ErrorMessage = "{0} zorunludur")]
        [Display(Name = "Değeri")]
        public string Value { get; set; }
    }
}
