using System.ComponentModel.DataAnnotations;

namespace Project.COREMVC.Areas.Admin.Models.PureVms.EntityAttribute
{
    public class UpdateAttributePureVm
    {
        public int AttributeID { get; set; }
        [Required(ErrorMessage = "{0} zorunludur")]
        [Display(Name = "Özellik ismi")]
        public string AttributeName { get; set; }

       
    }
}
