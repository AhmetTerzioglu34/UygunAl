using Project.COREMVC.Areas.Admin.Models.PureVms.EntityAttribute;

namespace Project.COREMVC.Areas.Admin.Models.PageVms.EntityAttribute
{
    public class EntityAttributePageVm
    {
        public List<EntityAttributePureVm> EntityAttributePureVms { get; set; }
        public List<EntityAttributeProductPureVm> EntityAttributeProductPureVms { get; set; }
        
    }
}
