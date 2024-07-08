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
    public class AppRoleRepository : BaseRepository<AppRole>, IAppRoleRepository
    {
        public AppRoleRepository( MyContext db) : base(db) 
        {

        }
    }
}
