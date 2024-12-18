using System.Text.Json.Serialization;

using Netplanety.Shared.Interfaces;
using Netplanety.Shared.Extensions;

namespace Netplanety.Integrations.IXC.Models;

internal readonly struct IXCClient : IClient
{
    [JsonInclude]
    internal string ClientId { get; init; }

    [JsonInclude]
    [JsonPropertyName("ativo")]
    internal string Active { get; init; }

    [JsonInclude]
    [JsonPropertyName("razao")]
    internal string Name { get; init; }

    [JsonPropertyName("cnpj_cpf")]
    public string CPF { get; init; }

    [JsonInclude]
    [JsonPropertyName("uf")]
    internal string State { get; init; }

    [JsonInclude]
    [JsonPropertyName("endereco")]
    internal string Address { get; init; }

    [JsonInclude]
    [JsonPropertyName("cidade")]
    internal string City { get; init; }

    [JsonInclude]
    [JsonPropertyName("bairro")]
    internal string Neighbourhood { get; init; }

    [JsonInclude]
    [JsonPropertyName("numero")]
    internal string Number { get; init; }

    [JsonInclude]
    internal string CEP { get; init; }

    [JsonInclude]
    [JsonPropertyName("data_nascimento")]
    internal string Birthday { get; init; }

    public int Id { get => int.TryParse(ClientId, out int id) ? id : field; init; }
    public string FirstName => Name.Split(' ')[0].Capitalize();
    public string LastName => Name.Split(' ')[^1].Capitalize();

    [JsonConstructor]
    internal IXCClient(
        string clientId,
        string active,
        string name,
        string cpf,
        string state,
        string address,
        string city,
        string neighbourhood,
        string number,
        string cep,
        string birthday)
    {
        ClientId = clientId;
        Active = active;
        Name = name;
        CPF = cpf;
        State = state;
        Address = address;
        City = city;
        Neighbourhood = neighbourhood;
        Number = number;
        CEP = cep;
        Birthday = birthday;
    }
}
