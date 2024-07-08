using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.ENTITIES.Models
{
    public class EntityAttributeProduct : BaseEntity
    {
        public int EntityAttributeID { get; set; }
        public int ProductID { get; set; }
        public string Value { get; set; }
        //Relatioanal Properties
        public virtual EntityAttribute EntityAttribute { get; set; }
        public virtual Product Product { get; set; }
    }
}
