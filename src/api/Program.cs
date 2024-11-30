using System.Text.Json;
using System.Text.Json.Serialization;

using Netplanety.Integrations.IXC;
using Netplanety.Shared.Interfaces;
using Netplanety.Integrations.IXC.Extensions;

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

#if DEBUG
		// Add and configure ERP integration
		builder.Services.AddIXCSoft(options =>
		{
			options.HttpClientOptions.Token = builder.Configuration["token"] ?? string.Empty;
			options.HttpClientOptions.BaseAddress = builder.Configuration["endpoint"] ?? string.Empty;
		});
#endif

		WebApplication app = builder.Build();
		app.UseHttpsRedirection();

		// Map endpoints
		app.MapControllers();
		app.MapOpenApi();
		app.Run();
	}
}
