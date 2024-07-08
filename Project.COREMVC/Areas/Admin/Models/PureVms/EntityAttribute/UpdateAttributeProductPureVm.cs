using System.ComponentModel.DataAnnotations;

namespace Project.COREMVC.Areas.Admin.Models.PureVms.EntityAttribute
{
    public class UpdateAttributeProductPureVm
    {
        public int EntityAttributeID { get; set; }
        public int ProductID { get; set; }
        

        [Required(ErrorMessage = "{0} zorunludur")]
        [Display(Name = "Değeri")]
        public string Value { get; set; }
    }
}
