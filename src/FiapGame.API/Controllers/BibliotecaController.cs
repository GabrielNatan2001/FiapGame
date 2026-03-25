using System.Security.Claims;
using FiapGame.Application.Jogo.Services;
using FiapGame.Shared.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FiapGame.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class BibliotecaController : ControllerBase
{
    [HttpPost("{jogoId:guid}")]
    public async Task<IActionResult> Adquirir(
        [FromServices] AdquirirJogoService service,
        [FromRoute] Guid jogoId)
    {
        var usuarioId = ObterUsuarioId();
        await service.Execute(usuarioId, jogoId);
        return NoContent();
    }

    [HttpGet]
    public async Task<IActionResult> Listar([FromServices] ListarBibliotecaService service)
    {
        var usuarioId = ObterUsuarioId();
        var biblioteca = await service.Execute(usuarioId);
        return Ok(biblioteca);
    }

    private Guid ObterUsuarioId()
    {
        var idClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(idClaim, out var usuarioId))
            throw new DomainException("Token inválido.");

        return usuarioId;
    }
}
