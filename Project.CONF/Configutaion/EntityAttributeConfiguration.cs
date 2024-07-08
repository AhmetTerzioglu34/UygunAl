using System;
using Project.ENTITIES.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Project.CONF.Configutaion
{
    public class EntityAttributeConfiguration : BaseConfiguration<EntityAttribute>
    {
        public override void Configure(EntityTypeBuilder<EntityAttribute> builder)
        {
            base.Configure(builder);

        }
    }
}
