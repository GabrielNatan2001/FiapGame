using FiapGame.Application.Usuario.Dtos;
using FiapGame.Application.Usuario.Services;
using Microsoft.AspNetCore.Mvc;

namespace FiapGame.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromServices] AutenticarUsuarioService service,
        [FromBody] AutenticarUsuarioDto.Request request)
    {
        var token = await service.Execute(request);
        return Ok(token);
    }
}
