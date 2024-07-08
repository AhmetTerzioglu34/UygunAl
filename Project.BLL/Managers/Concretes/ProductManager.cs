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
    public class ProductManager : BaseManager<Product> , IProductManager
    {
        readonly IProductRepository _iProductRep;
        public ProductManager(IProductRepository iProductRep) : base(iProductRep) 
        {
            _iProductRep = iProductRep;
        }
        public override List<Product> GetActives()
        {
            List<Product> products = _iProductRep.Where(x => x.UnitsInStock > 5);
            return products;
        }
    }
}
