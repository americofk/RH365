using D365_API_Nomina.Core.Application.Common.Interface;
using D365_API_Nomina.Infrastructure.Persistence;
using D365_API_Nomina.Infrastructure.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace D365_API_Nomina.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDBContext>(options =>
                    options.UseSqlServer(configuration["ConnectionStrings:Localhost"])
                    );

            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDBContext>());

            services.AddScoped<IEmailServices, EmailServices>();
            services.AddScoped<IConnectThirdServices, ConnectThirdServices>();
            return services;
        }
    }
}
