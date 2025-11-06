using Microsoft.AspNetCore.Localization;
using System.Globalization;
using TIVIT.CIPA.Api.Domain.Business;
using TIVIT.CIPA.Api.Domain.Interfaces.Business;
using TIVIT.CIPA.Api.Domain.Interfaces.Models;
using TIVIT.CIPA.Api.Domain.Interfaces.Repositories;
using TIVIT.CIPA.Api.Domain.Interfaces.Services;
using TIVIT.CIPA.Api.Domain.Interfaces.Services.Azure;
using TIVIT.CIPA.Api.Domain.Providers;
using TIVIT.CIPA.Api.Domain.Repositories;
using TIVIT.CIPA.Api.Domain.Repositories.Context;
using TIVIT.CIPA.Api.Domain.Services;
using TIVIT.CIPA.Api.Domain.Services.Azure;
using TIVIT.CIPA.Api.Domain.Settings;
using TIVIT.CIPA.Api.Security;

namespace TIVIT.CIPA.Api.Extensions
{
    public static class IServiceCollectionExtension
    {
        public static void ConfigureSettings(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.Configure<DatabaseSettings>(configuration.GetSection("DataBase"));
            services.Configure<NotificationSettings>(configuration.GetSection("Notification"));
          
            services.Configure<AuthSettings>(configuration.GetSection("Auth"));
        }

        public static void AddDi(this IServiceCollection services)
        {
            services.AddDbContext<CIPAContext>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<INotificationService, NotificationService>();

            services.AddScoped<IUserInfo, UserInfo>();

            services.AddSingleton<TokenProvider>();
            services.AddSingleton<OtpProvider>();
            services.AddSingleton<AuthTokenProvider>();
            services.AddSingleton<PasswordProvider>();

            services.AddScoped<IUserBusiness, UserBusiness>();
            services.AddScoped<IAuthBusiness, AuthBusiness>();
            services.AddScoped<IProfileBusiness, ProfileBusiness>();
            services.AddScoped<IActionBusiness, ActionBusiness>();
            services.AddScoped<IElectionBusiness, ElectionBusiness>();
            services.AddScoped<ICandidateBusiness, CandidateBusiness>();
            services.AddScoped<IVoterBusiness, VoterBusiness>();
            services.AddScoped<ISiteBusiness, SiteBusiness>();

            services.AddScoped<IActionRepository, ActionRepository>();
            services.AddScoped<IProfileRepository, ProfileRepository>();
            services.AddScoped<IUserPermissionRepository, UserPermissionRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IAuthRepository, AuthRepository>();

            services.AddScoped<IElectionRepository, ElectionRepository>();
            services.AddScoped<IElectionSiteRepository, ElectionSiteRepository>();
            services.AddScoped<ICandidateRepository, CandidateRepository>();
            services.AddScoped<IVoterRepository, VoterRepository>();

            services.AddScoped<ISiteRepository, SiteRepository>();

            services.AddScoped<IEmailService, EmailService>();

            
        }

        public static void ConfigureLocalization(this IServiceCollection services)
        {
            services.AddLocalization(o => { o.ResourcesPath = "Resources"; });

            services.Configure<RequestLocalizationOptions>(options =>
            {
                CultureInfo[] supportedCultures = new[]
                {
                    new CultureInfo("pt"),
                    new CultureInfo("es")
                };

                options.DefaultRequestCulture = new RequestCulture("pt");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
                options.RequestCultureProviders = new List<IRequestCultureProvider>
                {
                    new AcceptLanguageHeaderRequestCultureProvider()
                };
            });
        }
    }
}
