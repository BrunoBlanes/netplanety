using System.Text.Json.Serialization;

using Netplanety.Shared.Interfaces;

namespace Netplanety.Integrations.IXC.Models;

internal readonly struct IXCOnt : IOnt
{
    [JsonInclude]
    internal string OntId { get; }

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
    public int Id { get => int.TryParse(OntId, out int id) ? id : field; init; }

    [JsonConstructor]
    internal IXCOnt(
        string ontId,
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
        OntId = ontId;
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
}
