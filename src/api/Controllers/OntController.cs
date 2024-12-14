using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Netplanety.Shared.Interfaces;

namespace Netplanety.Api.Controllers;

[ApiController]
[Authorize("Employee")]
[Route("[controller]")]
public class OntController : ControllerBase
{
	private readonly ILogger<OntController> logger;
	private readonly IERPService erpService;

	public OntController(IERPService erpService, ILogger<OntController> logger)
	{
		this.logger = logger;
		this.erpService = erpService;
	}

	[HttpGet]
	[Authorize("Technical")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<IOnt>> GetOntAsync([FromQuery] int id, CancellationToken cancellationToken)
	{
		IOnt? fiberTerminal = await erpService.GetOntAsync(id, cancellationToken);
		return fiberTerminal is null ? NotFound() : Ok(fiberTerminal);
	}
}