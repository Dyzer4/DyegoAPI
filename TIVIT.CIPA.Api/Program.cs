using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Globalization;
using System.Reflection;
using TIVIT.CIPA.Api.Domain.Services;
using TIVIT.CIPA.Api.Extensions;
using TIVIT.CIPA.Api.Middleware;
using TIVIT.CIPA.Api.Security;
using TIVIT.CIPA.Api.Utility;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.ConfigureAuthentication();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("All", new AuthorizationPolicyBuilder()
           .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme, "AzureAd")
           .RequireAuthenticatedUser().Build());

    options.DefaultPolicy = options.GetPolicy("All")!;
});

builder.Services.AddRazorPages();
builder.Services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Permission", policyBuilder =>
    {
        policyBuilder.Requirements.Add(new PermissionAuthorizationRequirement());
    });
});

builder.Services.AddCors(o => o.AddPolicy("CorsPolicy", builder =>
{
    builder.WithOrigins(configuration.GetSection("Cors:AllowOrigins").Get<string[]>())
           .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowCredentials();
}));

builder.Services.AddRateLimiter(limiterOptions =>
{
    limiterOptions.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    limiterOptions.AddFixedWindowLimiter(policyName: "fixed", options =>
    {
        options.PermitLimit = 5;
        options.Window = TimeSpan.FromSeconds(10);
    });
});

builder.Services.AddHttpClient("default").ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
{
    ServerCertificateCustomValidationCallback = delegate { return true; }
});

builder.Services.AddDi();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "CIPA API", Description = "Swagger Projeto CIPA (TIVIT & SENAI)", Version = "v1" });
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

builder.Services.ConfigureSettings(configuration);

builder.Services.AddLocalization();
var localizationOptions = new RequestLocalizationOptions();
var supportedCultures = new[]
{
    new CultureInfo("pt"),
    new CultureInfo("es")
};

localizationOptions.SupportedCultures = supportedCultures;
localizationOptions.SupportedUICultures = supportedCultures;
localizationOptions.DefaultRequestCulture = new RequestCulture("pt");
localizationOptions.ApplyCurrentCultureToResponseHeaders = true;

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

app.UseRequestLocalization(localizationOptions);
app.UseCors("CorsPolicy");

app.UseSwagger();
app.UseSwaggerUI(o =>
{
    o.DocExpansion(DocExpansion.None);
});

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseExceptionHandler();
app.MapControllers();
app.Run();