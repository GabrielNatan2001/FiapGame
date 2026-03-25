using FiapGame.Application.Jogo.Dtos;
using FiapGame.Domain.Jogo.Interfaces;

namespace FiapGame.Application.Jogo.Services;

public class ListarJogosService
{
    private readonly IJogoRepository _jogoRepository;

    public ListarJogosService(IJogoRepository jogoRepository)
    {
        _jogoRepository = jogoRepository;
    }

    public async Task<IReadOnlyCollection<JogoItemDto>> Execute()
    {
        var jogos = await _jogoRepository.ObterTodos();
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
