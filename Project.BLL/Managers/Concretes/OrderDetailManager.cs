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
    public class OrderDetailManager : BaseManager<OrderDetail> , IOrderDetailManager
    {
        readonly IOrderDetailRepository _iODRep;
        public OrderDetailManager(IOrderDetailRepository iODRep) : base(iODRep) 
        {
            _iODRep = iODRep;
        }
    }
}
