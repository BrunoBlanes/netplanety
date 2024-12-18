using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;

using Netplanety.Shared.Identity;
using Netplanety.Shared.Interfaces;
using Netplanety.Shared.Models;

namespace Netplanety.Api.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize(AuthorizationPolicies.Identity)]
public class ClientController : ControllerBase
{
    private readonly IERPService _erpService;

    public ClientController(IERPService erpService)
    {
        _erpService = erpService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Client>> GetClientByCpfAsync([FromQuery] string cpf, CancellationToken cancellationToken)
    {
        Client? client = await _erpService.GetClientByCpfAsync(cpf, cancellationToken);
        return client is null ? NotFound() : Ok(client);
    }
}
