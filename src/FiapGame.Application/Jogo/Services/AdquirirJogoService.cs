using FiapGame.Domain.Common.Enums;
using FiapGame.Domain.Biblioteca.Entities;
using FiapGame.Domain.Biblioteca.Interfaces;
using FiapGame.Domain.Jogo.Entities;
using FiapGame.Domain.Jogo.Interfaces;
using FiapGame.Shared.Exceptions;

namespace FiapGame.Application.Jogo.Services;

public class AdquirirJogoService
{
    private readonly IJogoRepository _jogoRepository;
    private readonly IBibliotecaRepository _bibliotecaRepository;

    public AdquirirJogoService(IJogoRepository jogoRepository, IBibliotecaRepository bibliotecaRepository)
    {
        _jogoRepository = jogoRepository;
        _bibliotecaRepository = bibliotecaRepository;
    }

    public async Task Execute(Guid usuarioId, Guid jogoId)
    {
        var jogo = await _jogoRepository.ObterPorId(jogoId);
        if (jogo is null)
            throw new DomainException("Jogo não encontrado.");

        if (jogo.Status != EStatus.Ativo)
            throw new DomainException("Este jogo não está disponível para aquisição.");

        var biblioteca = await _bibliotecaRepository.ObterPorUsuarioId(usuarioId);
        
        if (biblioteca is null)
        {
            biblioteca = BibliotecaEntity.Criar(usuarioId);
            await _bibliotecaRepository.Adicionar(biblioteca);
        }

        biblioteca.AdicionarJogo(jogoId);
        await _bibliotecaRepository.SalvarAlteracoes();
    }
}
