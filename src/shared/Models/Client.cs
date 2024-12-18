using System.Diagnostics.CodeAnalysis;

using Netplanety.Shared.Interfaces;

namespace Netplanety.Shared.Models;

public readonly struct Client : IClient
{
    public required int Id { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string CPF { get; init; }

    public Client()
    {
        CPF = string.Empty;
        FirstName = string.Empty;
        LastName = string.Empty;
    }

    [SetsRequiredMembers]
    public Client(IClient client)
    {
        Id = client.Id;
        CPF = client.CPF;
        FirstName = client.FirstName;
        LastName = client.LastName;
    }
}
