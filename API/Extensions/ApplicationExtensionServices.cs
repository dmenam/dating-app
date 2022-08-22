using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Interfaces;
using API.Services;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public static class ApplicationExtensionServices
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config){
            
            //Service with a http request lifetime - dies after the http request is finished
            services.AddScoped<ITokenService, TokenService>();

            services.AddDbContext<DataContext> (options => {
                options.UseSqlite(config.GetConnectionString("DefaultConnection"));
            });

            return services;
        }
    }
}