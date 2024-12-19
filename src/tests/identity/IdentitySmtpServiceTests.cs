using Microsoft.Extensions.Logging;

using Netplanety.Identity.Services;
using Netplanety.Identity.Tests.Mocks;
using Netplanety.Shared.Models;
using Netplanety.Tests.Common.Mocks;

namespace Netplanety.Identity.Tests;

[TestClass]
public sealed class IdentitySmtpServiceTests
{
    private readonly ILogger<IdentitySmtpService> logger;
    private readonly MockSmtpService smtpService;

    public IdentitySmtpServiceTests()
    {
        smtpService = new MockSmtpService();
        logger = new MockLogger<IdentitySmtpService>();
    }

    [TestMethod]
    public async Task SendConfirmationLinkAsync_ExpectedResult()
    {
        var identitySmtpService = new IdentitySmtpService(smtpService, logger);
        var user = new ApplicationUser
        {

        };

        await identitySmtpService.SendConfirmationLinkAsync(user, "", "");
    }
}
