using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Project.DAL.ContextClasses;
using Project.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.ServiceInjection
{
    public static class IdentityService
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services)
        {
            services.AddIdentity<AppUser, AppRole>(x =>
            {
                x.Password.RequireDigit = false;
                x.Password.RequiredLength = 3;
                x.Password.RequireUppercase = false;
                x.Password.RequireLowercase = false;
                x.SignIn.RequireConfirmedEmail = true;
                x.Password.RequireNonAlphanumeric = false;
                x.Lockout.MaxFailedAccessAttempts = 5;
                x.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(2);
                //x.User.RequireUniqueEmail = true;

            }).AddDefaultTokenProviders().AddEntityFrameworkStores<MyContext>();
            return services;
        }
    }
       
}
