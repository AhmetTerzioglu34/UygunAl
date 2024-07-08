using Project.BLL.Managers.Abstracts;
using Project.DAL.Repositories.Abstracts;
using Project.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Managers.Concretes
{
    public class EntityAttributeProductManager : BaseManager<EntityAttributeProduct>, IEntityAttributeProductManager
    {
        readonly IEntityAttributeProductRepository _iEAProductRep;
        public EntityAttributeProductManager(IEntityAttributeProductRepository iEAProductRep) : base(iEAProductRep) 
        {
            _iEAProductRep = iEAProductRep;
        }
    }
}
