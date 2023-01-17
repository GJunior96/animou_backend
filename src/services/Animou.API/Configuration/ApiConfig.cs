using Animou.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using Microsoft.AspNetCore.Localization;

namespace Animou.API.Configuration
{
    public static class ApiConfig
    {
        public static void AddApiConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddLocalization(opt => opt.ResourcesPath = "Resources");

            services.AddDbContext<AnimouContext>(opt =>
                opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddControllers();
        }

        public static void UseApiConfiguration(this WebApplication app)
        {
            var supportedCultures = new[] { new CultureInfo("en-US"), new CultureInfo("pt-BR") };
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("en-US"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });
        }
    }
}
