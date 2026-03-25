using FiapGame.Application.Jogo.Dtos;
using FiapGame.Application.Jogo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FiapGame.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class JogoController : ControllerBase
{
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Listar([FromServices] ListarJogosService service)
    {
        var result = await service.Execute();
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Criar(
        [FromServices] CriarJogoService service,
        [FromBody] CriarJogoDto.Request request)
    {
        var id = await service.Execute(request);
        return CreatedAtAction(nameof(Listar), new { id }, new { id });
    }
}
