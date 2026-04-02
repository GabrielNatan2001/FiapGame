using FiapGame.Application.Jogo.Dtos;
using FiapGame.Domain.Jogo.Entities;
using FiapGame.Domain.Jogo.Interfaces;

namespace FiapGame.Application.Jogo.Services;

public class CriarJogoService
{
    private readonly IJogoRepository _jogoRepository;

    public CriarJogoService(IJogoRepository jogoRepository)
    {
        _jogoRepository = jogoRepository;
    }

    public async Task<Guid> Execute(CriarJogoDto.Request request)
    {
        var jogo = JogoEntity.Criar(request.Nome, request.Descricao, request.Preco, request.Categoria);
        await _jogoRepository.Adicionar(jogo);
        await _jogoRepository.SalvarAlteracoes();
        return jogo.Id;
    }
}
