using Project.BLL.Managers.Abstracts;
using Project.DAL.Repositories.Abstracts;
using Project.ENTITIES.Interfaces;
using Project.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Managers.Concretes
{
    public class EntityAttributeManager : BaseManager<EntityAttribute>, IEntityAttributeManager
    {
        readonly IEntityAttributeRepository _iEARep;
        public EntityAttributeManager(IEntityAttributeRepository iEARep) : base(iEARep) 
        {
            _iEARep = iEARep;
        }
    }
}
