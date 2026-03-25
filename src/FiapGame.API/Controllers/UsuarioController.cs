using FiapGame.Application.Usuario.Dtos;
using FiapGame.Application.Usuario.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FiapGame.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CriarUsuario([FromServices] CriarUsuarioService service, CriarUsuarioDto.Request request)
        {
            var result = await service.Execute(request);
            return Created();
        }
    }
}
