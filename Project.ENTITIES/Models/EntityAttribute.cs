using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.ENTITIES.Models
{
    public class EntityAttribute : BaseEntity
    {
        public string AttributeName { get; set; }

        //Relatioanal Properties
        public virtual ICollection<EntityAttributeProduct> EntityAttributeProducts { get; set; }

    }
}
