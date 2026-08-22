using Asp.Versioning;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace PortfolioEngine.Web.Controllers.Api;

[ApiController]
[Produces("application/json")]
[EnableCors(PortfolioEngine.Web.Extensions.ServiceCollectionExtensions.CorsPolicyName)]
[EnableRateLimiting(PortfolioEngine.Web.Extensions.ServiceCollectionExtensions.RateLimitingPolicyName)]
[Route("api/v{version:apiVersion}/[controller]")]
public abstract class BaseApiController : ControllerBase
{
}