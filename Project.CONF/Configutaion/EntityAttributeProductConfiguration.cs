using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.CONF.Configutaion
{
    public class EntityAttributeProductConfiguration : BaseConfiguration<EntityAttributeProduct>
    {
        public override void Configure(EntityTypeBuilder<EntityAttributeProduct> builder)
        {
            base.Configure(builder);
            builder.Ignore(x=>x.ID);
            builder.HasKey(x => new
            {
                x.EntityAttributeID,
                x.ProductID
            });
        }
    }
}
