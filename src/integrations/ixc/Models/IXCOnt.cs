using System.Text.Json.Serialization;

using Microsoft.Extensions.Primitives;

using Netplanety.Shared.Interfaces;
using Netplanety.Shared.Models;

namespace Netplanety.Integrations.IXC.Models;

internal readonly struct IXCOnt
{
	[JsonInclude]
	internal string Id { get; }

	[JsonInclude]
	[JsonPropertyName("id_contrato")]
	internal string ContractId { get; }

	[JsonInclude]
	[JsonPropertyName("id_projeto")]
    internal string ProjectId { get; }

	[JsonInclude]
	[JsonPropertyName("id_transmissor")]
	internal string TransmissorId { get; }

	[JsonInclude]
	[JsonPropertyName("nome")]
	internal string Name { get; }

	[JsonInclude]
	[JsonPropertyName("id_caixa_ftth")]
	internal string OpticalTerminationBoxId { get; }

	[JsonInclude]
	[JsonPropertyName("porta_ftth")]
	internal string Port { get; }

	[JsonInclude]
	[JsonPropertyName("id_login")]
	internal string LoginId { get; }

	[JsonInclude]
	[JsonPropertyName("ponid")]
	internal string PonId { get; }

	[JsonInclude]
	internal string MAC { get; }

	[JsonInclude]
	[JsonPropertyName("sinal_rx")]
	internal string RxSignal { get; }

	[JsonInclude]
	[JsonPropertyName("sinal_tx")]
	internal string TxSignal { get; }

	[JsonInclude]
	[JsonPropertyName("temperatura")]
	internal string Temperature { get; }

	[JsonInclude]
	[JsonPropertyName("voltagem")]
	internal string Voltage { get; }

	[JsonInclude]
	[JsonPropertyName("id_perfil")]
	internal string ProfileId { get; }

	[JsonInclude]
	internal string VLAN { get; }

	[JsonConstructor]
	internal IXCOnt(
		string id,
		string contractId,
		string projectId,
		string transmissorId,
		string name,
		string opticalTerminationBoxId,
		string port,
		string loginId,
		string ponId,
		string mac,
		string rxSignal,
		string txSignal,
		string temperature,
		string voltage,
		string profileId,
		string vlan)
	{
		Id = id;
		ContractId = contractId;
		ProjectId = projectId;
		TransmissorId = transmissorId;
		Name = name;
		OpticalTerminationBoxId = opticalTerminationBoxId;
		Port = port;
		LoginId = loginId;
		PonId = ponId;
		MAC = mac;
		RxSignal = rxSignal;
		TxSignal = txSignal;
		Temperature = temperature;
		Voltage = voltage;
		ProfileId = profileId;
		VLAN = vlan;
	}

	/// <summary>
	/// Convert a <see cref="IXCOnt"/> object into an <see cref="IOnt"/>.
	/// </summary>
	/// <returns>
	/// A new instance of <see cref="IOnt"/> object with the same properties of this <see cref="IXCOnt"/> object.
	/// </returns>
	internal IOnt ToOnt()
	{
		return new Ont
		{
			Id = int.TryParse(this.Id, out var id) ? id : default,
			ContractId = int.TryParse(this.ContractId, out var contractId) ? contractId : default,
			ProjectId = this.ProjectId,
			TransmissorId = this.TransmissorId,
			Name = this.Name.Trim(),
			OpticalTerminationBoxId = this.OpticalTerminationBoxId,
			Port = this.Port,
			LoginId = this.LoginId,
			PonId = this.PonId,
			MAC = this.MAC,
			RxSignal = double.TryParse(this.RxSignal, out var rxSignal) ? rxSignal : default,
			TxSignal = double.TryParse(this.TxSignal, out var txSignal) ? txSignal : default,
			Temperature = float.TryParse(this.Temperature, out var temperature) ? temperature : default,
			Voltage = float.TryParse(this.Voltage, out var voltage) ? voltage : default,
			ProfileId = this.ProfileId,
			VLAN = this.VLAN
		};
	}
}
