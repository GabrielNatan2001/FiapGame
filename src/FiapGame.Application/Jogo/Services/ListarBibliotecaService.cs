using FiapGame.Application.Jogo.Dtos;
using FiapGame.Domain.Jogo.Interfaces;

namespace FiapGame.Application.Jogo.Services;

public class ListarBibliotecaService
{
    private readonly IUsuarioJogoRepository _usuarioJogoRepository;

    public ListarBibliotecaService(IUsuarioJogoRepository usuarioJogoRepository)
    {
        _usuarioJogoRepository = usuarioJogoRepository;
    }

    public async Task<IReadOnlyCollection<JogoItemDto>> Execute(Guid usuarioId)
    {
        var jogos = await _usuarioJogoRepository.ObterBiblioteca(usuarioId);
        return jogos
            .Select(x => new JogoItemDto
            {
                Id = x.Id,
                Titulo = x.Titulo,
                Descricao = x.Descricao,
                Preco = x.Preco
            })
            .ToList();
    }
}
