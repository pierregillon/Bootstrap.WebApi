using System.Reflection;
using Microsoft.AspNetCore.Mvc;

namespace Bootstrap.WebApi.Controllers.V1;

[ApiController]
[Route("v{v:apiVersion}/version")]
[ApiVersion("1.0")]
public class VersionController : ControllerBase
{
    [HttpGet]
    public string? Get() => Assembly.GetEntryAssembly()?.GetName().Version?.ToString();
}
