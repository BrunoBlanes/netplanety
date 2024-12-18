using System.Text.Json;
using System.Text.Json.Serialization;

using Microsoft.AspNetCore.Authentication.JwtBearer;

using Netplanety.Shared.Extensions;
using Netplanety.Shared.Identity;
using System.Net;



#if INCLUDE_IXC
using Netplanety.Integrations.IXC.Extensions;
#endif

namespace Netplanety.Api;

public partial class Program
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateEmptyBuilder(new WebApplicationOptions());

        builder.Configuration.AddJsonFile("appsettings.json", true);
#if DEBUG
        builder.Configuration.AddJsonFile("appsettings.Development.json", true);
#endif

        builder.WebHost.UseKestrelCore().ConfigureKestrel(options =>
        {
            int port = builder.Configuration.GetValue<int>("host:port");

            if (builder.Configuration["host:address"] is string address)
            {
                options.Listen(IPAddress.Parse(address), port);
            }

            else
            {
                options.ListenLocalhost(port);
            }
        });

        builder.Logging.AddConsole();

        // Add support for controllers and configure JSON output
        builder.Services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.WriteIndented = true;
            options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.JsonSerializerOptions.PreferredObjectCreationHandling = JsonObjectCreationHandling.Populate;
        });

        // Configure endpoints to be case invariant
        builder.Services.AddRouting(options =>
        {
            options.LowercaseUrls = true;
            options.LowercaseQueryStrings = true;
        });

        // Add OpenAPI support
        builder.Services.AddOpenApi();

        // Add and configure ERP integration
#if INCLUDE_IXC
        builder.Services.AddIXCSoft(options =>
        {
            options.HttpClientOptions.Token = builder.Configuration["ixc:token"] ?? string.Empty;
            options.HttpClientOptions.BaseAddress = builder.Configuration["ixc:endpoint"] ?? string.Empty;
        });
#endif

        // Add JSON Web Token authentication scheme
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
        {
            // Sets the token authority and type
            options.Authority = builder.Configuration["authentication:authority"];
            options.TokenValidationParameters.ValidTypes = ["at+jwt"];

            // Validates the audience claim
            options.TokenValidationParameters.ValidateAudience = true;
            options.TokenValidationParameters.ValidAudiences = [ApiResources.Clients];
        });

        // Add authorization polices
        builder.Services.AddAuthorizationBuilder().AddPolicy(AuthorizationPolicies.Identity, policy =>
        {
            policy.RequireScope(IdentityScopes.Read);
        });

        WebApplication app = builder.Build();
        app.UseHttpsRedirection();

        // Adds authentication and authorization
        app.UseAuthentication();
        app.UseAuthorization();

        // Map endpoints
        app.MapControllers();
        app.MapOpenApi();
        app.Run();
    }
}
