using Netplanety.Shared.Services.Smtp;

namespace Netplanety.Identity.Tests.Mocks;

internal class MockSmtpService : ISmtpService
{
    public MockSmtpService()
    {

    }

    public Task SendAsync(string user, string address, string subject, string content, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public ValueTask DisposeAsync()
    {
        return ValueTask.CompletedTask;
    }
}
