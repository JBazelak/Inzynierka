using Inzynierka.Application.Interfaces;
using Inzynierka.Application.Services;
using Inzynierka.Infrastructure.Persistance;
using Inzynierka.Infrastructure.Repositories;
using Inzynierka.Infrastructure.Validators;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Inzynierka.Infrastructure.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Rejestracja DbContext
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                sqlServerOptions => sqlServerOptions.EnableRetryOnFailure()));

            services.AddScoped<IContractorRepository, ContractorRepository>();
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IMaterialRepository, MaterialRepository>();
            services.AddScoped<IProjectRepository, ProjectRepository>();

            // Rejestracja serwisów aplikacyjnych
            services.AddScoped<IMaterialService, MaterialService>();
            services.AddScoped<IProjectService, ProjectService>();  

            // Rejestracja walidatorów
            services.AddScoped<ICompanyDataValidator, CompanyDataValidator>();
            services.AddScoped<IMaterialValidator, MaterialValidator>();
        }
    }
}
