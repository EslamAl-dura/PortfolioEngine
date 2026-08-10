using Microsoft.AspNetCore.Mvc;

namespace PortfolioEngine.Web.Controllers.Api;

[ApiController]
[Route("api/v1/[controller]")]
public abstract class BaseApiController : ControllerBase
{
}