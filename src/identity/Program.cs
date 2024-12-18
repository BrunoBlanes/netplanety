using System.Reflection;

using Duende.IdentityServer.Configuration;
using Duende.IdentityServer.EntityFramework.DbContexts;

using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using Netplanety.Identity.Components;
using Netplanety.Identity.Components.Account;
using Netplanety.Identity.Data;
using Netplanety.Identity.Extensions;
using Netplanety.Identity.Services;
using Netplanety.Shared.Extensions;
using Netplanety.Shared.Identity;
using Netplanety.Shared.Models;
using Netplanety.Shared.Services.Api;

namespace Netplanety.Identity;

public class Program
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
        string? dbConnectionString = builder.Configuration.GetConnectionString("database");

        // Add services to the container.
        builder.Services.AddRazorComponents().AddInteractiveServerComponents();

        // Configure authentication
        builder.Services.AddCascadingAuthenticationState();
        builder.Services.AddScoped<IdentityUserAccessor>();
        builder.Services.AddScoped<IdentityRedirectManager>();
        builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

        // Add database support using EF Core
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(dbConnectionString);
        });

#if DEBUG
		// Generate migration exception HTML help page
		builder.Services.AddDatabaseDeveloperPageExceptionFilter();
#endif

        // Add SMTP service
        builder.Services.AddSmtpClient(options =>
        {
            options.Host = builder.Configuration["smtp:host"]!;
            options.User = builder.Configuration["smtp:user"]!;
            options.Password = builder.Configuration["smtp:password"]!;
            options.FromName = builder.Configuration["smtp:fromname"]!;
            options.FromAddress = builder.Configuration["smtp:fromaddress"]!;
            options.LocalDomain = builder.Configuration["smtp:localdomain"]!;
        }).AddIdentitySupport();

        // Configure default authentication scheme
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultScheme = IdentityConstants.ApplicationScheme;
            options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
        }).AddIdentityCookies();

        builder.Services.AddIdentityCore<ApplicationUser>(options =>
        {
            // Configure the application user
            options.User.RequireUniqueEmail = true;

            // Set sign in requirements
            options.SignIn.RequireConfirmedEmail = true;
            options.SignIn.RequireConfirmedAccount = true;

            // Maximum number of attempts at login before lockout
            options.Lockout.MaxFailedAccessAttempts = 3;

            // Define password minimum requirements
            options.Password.RequiredLength = 8;
        }).AddEntityFrameworkStores<ApplicationDbContext>().AddSignInManager().AddDefaultTokenProviders();

        // Add and configure the Duende Identity Server
        builder.Services.AddIdentityServer(options =>
        {
            // Disable unused endpoints
            options.Endpoints.EnableTokenEndpoint = true;
            options.Endpoints.EnableCheckSessionEndpoint = false;
            options.Endpoints.EnableIntrospectionEndpoint = false;
            options.Endpoints.EnablePushedAuthorizationEndpoint = false;
            options.Endpoints.EnableBackchannelAuthenticationEndpoint = false;

            // Hide unused properties from the discovery endpoint
            options.Discovery.ShowTokenEndpointAuthenticationMethods = false;
            options.Discovery.ShowExtensionGrantTypes = false;
            options.Discovery.ShowResponseModes = false;

            // Configure the server properties
            options.IssuerUri = builder.Configuration["identity:server:issuer"];
            options.LicenseKey = builder.Configuration["identity:server:license"];
            options.EmitIssuerIdentificationResponseParameter = true;
            options.StrictJarValidation = true;

            // Set the ASP.NET Core Identity UI pages
            options.UserInteraction.CreateAccountUrl = "/register";
            options.UserInteraction.ErrorUrl = "/error";
            options.UserInteraction.LoginUrl = "/login";
            options.UserInteraction.LogoutUrl = "/logout";

            // New key every 14 days
            options.KeyManagement.RotationInterval = TimeSpan.FromDays(14);

            // Announce new key 7 days in advance in discovery
            options.KeyManagement.PropagationTime = TimeSpan.FromDays(7);

            // Keep old key for 7 days in discovery for validation of tokens
            options.KeyManagement.RetentionDuration = TimeSpan.FromDays(7);

            options.KeyManagement.SigningAlgorithms = new[]
            {
                new SigningAlgorithmOptions(SecurityAlgorithms.RsaSsaPssSha512)
            };
        })
        .AddConfigurationStore<ConfigurationDbContext>(options =>
        {
            options.ConfigureDbContext = builder =>
            {
                builder.UseSqlServer(dbConnectionString, sql => sql.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName));
            };
        })
        .AddOperationalStore<PersistedGrantDbContext>(options =>
        {
            options.ConfigureDbContext = builder =>
            {
                builder.UseSqlServer(dbConnectionString, sql => sql.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName));
            };
        })
        .AddAspNetIdentity<ApplicationUser>();

        // Add token management and client configuration for the Netplanety API
        builder.Services.AddClientCredentialsTokenManagement().AddClient("identity", client =>
        {
            client.ClientId = GuidExtensions.NewGuid(builder.Configuration.GetValue<int>("identity:client:id")).ToString();
            client.TokenEndpoint = "/connect/token";
            client.HttpClient = new HttpClient
            {
                BaseAddress = new Uri(builder.Configuration["identity:server:issuer"] ?? string.Empty)
            };

            client.ClientSecret = builder.Configuration["identity:client:secret"];
            client.Scope = IdentityScopes.Read.ToLowerInvariantString();
            client.Resource = ApiResources.Clients;
        });

        // Register the API service and add a typed httpclient
        builder.Services.AddSingleton<IApiService, ApiService>();
        builder.Services.AddHttpClient<IApiService, ApiService>(client =>
        {
            client.BaseAddress = new Uri(builder.Configuration["api:domain"] ?? string.Empty);
        }).AddClientCredentialsTokenHandler("identity");

        // Add data protection
        builder.Services.AddDataProtection();
        WebApplication app = builder.Build();

#if DEBUG
		// This is a temporary solution for applying
		// migrations until a better one is developed later on
		app.ApplyMigrations();
		app.UseDeveloperExceptionPage();
#endif

        // Configure the HTTP request pipeline.
        app.UseExceptionHandler("/error");

        // The default HSTS value is 30 days.
        // You may want to change this for production scenarios
        // see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
        app.UseHttpsRedirection();
        app.UseAntiforgery();

        app.MapStaticAssets();

        // Add Identity Server middleware
        app.UseIdentityServer();
        app.UseAuthorization();

        app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

        // Add additional endpoints required by the Identity Razor components.
        app.MapAdditionalIdentityEndpoints();
        app.Run();
    }
}