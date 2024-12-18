using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Mappers;
using Duende.IdentityServer.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using Netplanety.Identity.Data;
using Netplanety.Shared.Extensions;
using Netplanety.Shared.Identity;

namespace Netplanety.Identity.Extensions;

public static class WebApplicationExtensions
{
    public static void ApplyMigrations(this WebApplication app)
    {
        using IServiceScope scope = app.Services.CreateScope();

        using (ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>())
        {
            dbContext.Database.Migrate();
        };

        using (PersistedGrantDbContext persistedGrantDbContext = scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>())
        {
            persistedGrantDbContext.Database.Migrate();
        }

        using ConfigurationDbContext configurationDbContext = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
        configurationDbContext.Database.Migrate();

        if (configurationDbContext.Clients.Any() is false)
        {
            _ = configurationDbContext.Clients.Add(
                new Client
                {
                    ClientId = GuidExtensions.NewGuid(app.Configuration.GetValue<int>("identity:client:id")).ToString(),
                    ClientSecrets = { new Secret(app.Configuration["identity:client:secret"].Sha512()) },
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = IdentityScopes.Read.ToStringCollection()
                }.ToEntity()
            );

            _ = configurationDbContext.ApiResources.Add(
                new ApiResource(ApiResources.Clients, "Clients API")
                {
                    RequireResourceIndicator = true,
                    Scopes = IdentityScopes.Read.ToStringCollection(),
                    AllowedAccessTokenSigningAlgorithms = { SecurityAlgorithms.RsaSsaPssSha512 }
                }.ToEntity()
            );

            _ = configurationDbContext.ApiScopes.Add(
                new ApiScope(IdentityScopes.Read.ToLowerInvariantString(), IdentityScopes.Read.ToString()).ToEntity()
            );
            _ = configurationDbContext.SaveChanges();
        }
    }
}
