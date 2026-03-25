using FiapGame.Domain.Jogo.Entities;
using FiapGame.Domain.Jogo.Interfaces;
using FiapGame.Shared.Exceptions;

namespace FiapGame.Application.Jogo.Services;

public class AdquirirJogoService
{
    private readonly IJogoRepository _jogoRepository;
    private readonly IUsuarioJogoRepository _usuarioJogoRepository;

    public AdquirirJogoService(IJogoRepository jogoRepository, IUsuarioJogoRepository usuarioJogoRepository)
    {
        _jogoRepository = jogoRepository;
        _usuarioJogoRepository = usuarioJogoRepository;
    }

    public async Task Execute(Guid usuarioId, Guid jogoId)
    {
        var jogo = await _jogoRepository.ObterPorId(jogoId);
        if (jogo is null)
            throw new DomainException("Jogo não encontrado.");

        if (await _usuarioJogoRepository.UsuarioPossuiJogo(usuarioId, jogoId))
            throw new DomainException("Jogo já adquirido para este usuário.");

        await _usuarioJogoRepository.Adicionar(UsuarioJogoEntity.Criar(usuarioId, jogoId));
        await _usuarioJogoRepository.SalvarAlteracoes();
    }
}
