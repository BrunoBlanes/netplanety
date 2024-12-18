using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;

using Netplanety.Shared.Identity;
using Netplanety.Shared.Interfaces;

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
	public async Task<ActionResult<IClient>> GetClientByCpfAsync([FromQuery] string cpf, CancellationToken cancellationToken)
	{
		IClient? client = await erpService.GetClientByCpfAsync(cpf, cancellationToken);
		return client is null ? NotFound() : Ok(client);
	}
}
