using System.Text.Json.Serialization;

using Netplanety.Shared.Interfaces;

namespace Netplanety.Shared.Models;

public struct Ont : IOnt
{
	public int Id { get; set; }
	public int ContractId { get; set; }
	public string ProjectId { get; set; }
	public string TransmissorId { get; set; }
	public string Name { get; set; }
	public string OpticalTerminationBoxId { get; set; }
	public string Port { get; set; }
	public string LoginId { get; set; }
	public string PonId { get; set; }
	public string MAC { get; set; }
	public double RxSignal { get; set; }
	public double TxSignal { get; set; }
	public float Temperature { get; set; }
	public float Voltage { get; set; }
	public string ProfileId { get; set; }
	public string VLAN { get; set; }
}
