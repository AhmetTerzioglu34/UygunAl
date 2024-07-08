using System.ComponentModel.DataAnnotations;

namespace Project.COREMVC.Areas.Admin.Models.PureVms.Category
{
    public class UpdateCategoryPureVm
    {

        public int ID { get; set; }
        [Required(ErrorMessage = "{0} zorunludur")]
        [Display(Name = "Kategori ismi")]
        [MaxLength(20, ErrorMessage = "{0} en fazla {1} karakter alabilir")]
        [MinLength(2, ErrorMessage = "{0} en az {1} karakter alabilir")]
        public string CategoryName { get; set; }

        [Required(ErrorMessage = "{0} zorunludur")]
        [Display(Name = "Açıklama ")]
        [MaxLength(500, ErrorMessage = "{0} en fazla {1} karakter alabilir")]
        [MinLength(5, ErrorMessage = "{0} en az {1} karakter alabilir")]
        public string Description { get; set; }
    }
}
