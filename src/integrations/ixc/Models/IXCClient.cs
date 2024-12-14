using System.Text.Json.Serialization;

using Netplanety.Shared.Interfaces;
using Netplanety.Shared.Models;

namespace Netplanety.Integrations.IXC.Models;

internal readonly struct IXCClient
{
	[JsonInclude]
	internal string Id { get; }

	[JsonInclude]
	[JsonPropertyName("ativo")]
	internal string Active { get; }

	[JsonInclude]
	[JsonPropertyName("razao")]
	internal string Name { get; }

	[JsonInclude]
	[JsonPropertyName("cnpj_cpf")]
	internal string CPF { get; }

	[JsonInclude]
	[JsonPropertyName("uf")]
	internal string State { get; }

	[JsonInclude]
	[JsonPropertyName("endereco")]
	internal string Address { get; }

	[JsonInclude]
	[JsonPropertyName("cidade")]
	internal string City { get; }

	[JsonInclude]
	[JsonPropertyName("bairro")]
	internal string Neighbourhood { get; }

	[JsonInclude]
	[JsonPropertyName("numero")]
	internal string Number { get; }

	[JsonInclude]
	internal string CEP { get; }

	[JsonInclude]
	[JsonPropertyName("data_nascimento")]
	internal string Birthday { get; }

	[JsonConstructor]
	internal IXCClient(
		string id,
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
		Id = id;
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

	internal IClient ToClient()
	{
		return new Client
		{
			Name = this.Name,
			IsActive = this.Active == "S",
			Id = int.TryParse(this.Id, out var id) ? id : default,
			CPF = this.CPF,
			Birthday = DateTime.Parse(this.Birthday)
		};
	}
}
