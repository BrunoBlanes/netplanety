using System.Text.Json;
using System.Text.Json.Serialization;
using Netplanety.Integrations.IXC.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Netplanety.Shared.Extensions;
using Netplanety.Shared.Identity;

namespace Netplanety.Api;

public partial class Program
{
	public static void Main(string[] args)
	{
		WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

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
		builder.Services.AddIXCSoft(options =>
		{
			options.HttpClientOptions.Token = builder.Configuration["token"] ?? string.Empty;
			options.HttpClientOptions.BaseAddress = builder.Configuration["endpoint"] ?? string.Empty;
		});

		// Add JSON Web Token authentication scheme
		builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
		{
			// Sets the token authority and type
			options.Authority = builder.Configuration["authentication:authority"];
			options.TokenValidationParameters.ValidTypes = new[] { "at+jwt" };

			// Validates the audience claim
			options.TokenValidationParameters.ValidateAudience = true;
			options.TokenValidationParameters.ValidAudiences = [ ApiResources.Clients ];
		});

		// Add authorization polices
		builder.Services.AddAuthorization(options =>
		{ 
			options.AddPolicy(AuthorizationPolicies.Identity, policy =>
			{
				policy.RequireScope(IdentityScopes.Read);
			});
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
