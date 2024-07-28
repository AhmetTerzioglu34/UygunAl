using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Project.DAL.ContextClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.ServiceInjection
{
    public static class DbContextService
    {
        public static IServiceCollection AddDbContextService(this IServiceCollection services)
        {
            //Biz yapıları ayaga kaldırmak adına bir giriş noktasına ihtiyac duyarız ve bu giriş noktasını bana ServiceProvider nesnesi saglar.
            ServiceProvider provider = services.BuildServiceProvider();

            //IConfiguration sayesinde projenizin conf.(ayarlamalarının) bulundugu dosyaya ulasabiliyorsunuz.
            IConfiguration? configuration = provider.GetService<IConfiguration>();


            services.AddDbContextPool<MyContext>(opt => opt.UseSqlServer(configuration.GetConnectionString("MyConnection")).UseLazyLoadingProxies());
            return services;
        }
    }
}
