using Microsoft.Extensions.DependencyInjection;
using Project.DAL.Repositories.Abstracts;
using Project.DAL.Repositories.Concrates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.ServiceInjection
{
    public static class RepositoryService
    {
       public static IServiceCollection AddRepositoryService(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));

            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
            services.AddScoped<IAppUserRepository, AppUserRepository>();
            services.AddScoped<IProfileRepository, ProfileRepository>();
            services.AddScoped<IEntityAttributeRepository, EntityAttributeRepository>();
            services.AddScoped<IEntityAttributeProductRepository, EntityAttributeProductRepository>();
            services.AddScoped<IAppUserRepository, AppUserRepository>();
            services.AddScoped<IAppUserRoleRepository, AppUserRoleRepository>();
            return services;

        }
    }
}
