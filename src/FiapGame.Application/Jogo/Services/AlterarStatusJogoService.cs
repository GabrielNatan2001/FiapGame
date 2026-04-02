using FiapGame.Domain.Jogo.Interfaces;
using FiapGame.Shared.Exceptions;

namespace FiapGame.Application.Jogo.Services;

public class AlterarStatusJogoService
{
    private readonly IJogoRepository _jogoRepository;

    public AlterarStatusJogoService(IJogoRepository jogoRepository)
    {
        _jogoRepository = jogoRepository;
    }

    public async Task Execute(Guid id)
    {
        var jogo = await _jogoRepository.ObterPorId(id);
        if (jogo is null)
            throw new DomainException("Jogo não encontrado.");

        jogo.AlterarStatus();
        
        _jogoRepository.Atualizar(jogo);
        await _jogoRepository.SalvarAlteracoes();
    }
}
