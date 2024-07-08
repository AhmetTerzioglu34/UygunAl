using Microsoft.Extensions.DependencyInjection;
using Project.BLL.Managers.Abstracts;
using Project.BLL.Managers.Concretes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.ServiceInjection
{
    public static class ManagerService
    {
        public static IServiceCollection AddManagerServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IManager<>), typeof(BaseManager<>));

            services.AddScoped<IProductManager , ProductManager>();
            services.AddScoped<IAppUserManager , AppUserManager>();
            services.AddScoped<IProfileManager , ProfileManager>();
            services.AddScoped<ICategoryManager , CategoryManager>();
            services.AddScoped<IEntityAttributeManager , EntityAttributeManager>();
            services.AddScoped<IEntityAttributeProductManager , EntityAttributeProductManager>();
            services.AddScoped<IOrderManager , OrderManager>();
            services.AddScoped<IOrderDetailManager , OrderDetailManager>();
            services.AddScoped<IAppUserManager , AppUserManager>();
            services.AddScoped<IAppUserRoleManager , AppUserRoleManager>();

            return services;
        }
    }
}
