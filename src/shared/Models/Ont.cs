using Netplanety.Shared.Interfaces;

namespace Netplanety.Shared.Models;

public readonly struct Ont : IOnt
{
    public int Id { get; init; }

    public Ont(IOnt ont)
    {
        Id = ont.Id;
    }
}
