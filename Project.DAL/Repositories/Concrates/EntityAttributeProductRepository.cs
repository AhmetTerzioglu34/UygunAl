using Project.DAL.ContextClasses;
using Project.DAL.Repositories.Abstracts;
using Project.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.DAL.Repositories.Concrates
{
    public class EntityAttributeProductRepository : BaseRepository<EntityAttributeProduct>,IEntityAttributeProductRepository    {
        public EntityAttributeProductRepository(MyContext db) : base(db)
        {

        }
    }
}
