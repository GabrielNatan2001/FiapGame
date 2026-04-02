using FiapGame.Application.Jogo.Dtos;
using FiapGame.Domain.Common.Enums;
using FiapGame.Domain.Jogo.Interfaces;

namespace FiapGame.Application.Jogo.Services;

public class ListarJogosAtivosService
{
    private readonly IJogoRepository _jogoRepository;

    public ListarJogosAtivosService(IJogoRepository jogoRepository)
    {
        _jogoRepository = jogoRepository;
    }

    public async Task<IReadOnlyCollection<JogoItemDto>> Execute()
    {
        var jogos = await _jogoRepository.ObterTodos();
        return jogos
            .Where(x => x.Status == EStatus.Ativo)
            .Select(x => new JogoItemDto
            {
                Id = x.Id,
                Nome = x.Nome,
                Descricao = x.Descricao,
                Preco = x.Preco,
                Categoria = x.Categoria,
                Status = x.Status
            })
            .ToList();
    }
}
