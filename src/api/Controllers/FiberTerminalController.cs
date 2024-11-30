using Microsoft.AspNetCore.Mvc;

using Netplanety.Shared.Interfaces;
using Netplanety.Shared.Models;

namespace Netplanety.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class FiberTerminalController : ControllerBase
{
	private readonly ILogger<FiberTerminalController> logger;
	private readonly IERPService erpService;

	public FiberTerminalController(IERPService erpService, ILogger<FiberTerminalController> logger)
	{
		this.logger = logger;
		this.erpService = erpService;
	}

	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<IFiberTerminal>> GetFiberTerminalAsync([FromQuery] int id, CancellationToken cancellationToken)
	{
		IFiberTerminal? fiberTerminal = await erpService.GetFiberTerminalAsync(id, cancellationToken);
		return fiberTerminal is null ? NotFound() : Ok(fiberTerminal);
	}
}