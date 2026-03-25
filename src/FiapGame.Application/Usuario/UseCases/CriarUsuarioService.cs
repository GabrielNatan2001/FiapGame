using FiapGame.Application.Usuario.Dtos;
using FiapGame.Domain.Usuario.Entities;
using FiapGame.Domain.Usuario.Interfaces;
using FiapGame.Shared.Exceptions;
using Microsoft.Extensions.Logging;

namespace FiapGame.Application.Usuario.Services;

public class CriarUsuarioService
{
    private readonly IUsuarioRepository _repository;
    private readonly ILogger<CriarUsuarioService> _logger;

    public CriarUsuarioService(IUsuarioRepository repository, ILogger<CriarUsuarioService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Guid> Execute(CriarUsuarioDto.Request request)
    {
        var existente = await _repository.ObterPorEmailAsync(request.Email);
        _logger.LogInformation("Validando se email ja existe.");
        if (existente is not null)
            throw new DomainException("Email já cadastrado");

        _logger.LogInformation("E-mail não cadastrado, criando usuario.");
        var usuario = UsuarioEntity.Criar(
            request.Nome,
            request.Email,
            request.Senha
        );
        
        await _repository.Adicionar(usuario);
        await _repository.SalvarAlteracoes();
        _logger.LogInformation("Usuario cadastrado com sucesso.");
        return usuario.Id;
    }
}
