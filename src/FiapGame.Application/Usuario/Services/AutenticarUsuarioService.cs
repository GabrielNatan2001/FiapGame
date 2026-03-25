using FiapGame.Application.Abstractions.Security;
using FiapGame.Application.Usuario.Dtos;
using FiapGame.Domain.Usuario.Interfaces;
using FiapGame.Shared.Exceptions;
using Microsoft.Extensions.Logging;

namespace FiapGame.Application.Usuario.Services;

public class AutenticarUsuarioService
{
    private readonly IUsuarioRepository _repository;
    private readonly ITokenProvider _tokenProvider;
    private readonly ILogger<AutenticarUsuarioService> _logger;

    public AutenticarUsuarioService(
        IUsuarioRepository repository,
        ITokenProvider tokenProvider,
        ILogger<AutenticarUsuarioService> logger)
    {
        _repository = repository;
        _tokenProvider = tokenProvider;
        _logger = logger;
    }

    public async Task<AutenticarUsuarioDto.Response> Execute(AutenticarUsuarioDto.Request request)
    {
        _logger.LogInformation("Iniciando autenticacao de usuario.");
        var usuario = await _repository.ObterPorEmailAsync(request.Email);

        if (usuario is null || !usuario.Password.Verify(request.Senha))
        {
            _logger.LogWarning("Falha de autenticacao para o email informado.");
            throw new DomainException("Email ou senha inválidos.");
        }

        var accessToken = _tokenProvider.GerarToken(usuario);

        return new AutenticarUsuarioDto.Response
        {
            AccessToken = accessToken,
            ExpiraEmUtc = DateTime.UtcNow.AddHours(8)
        };
    }
}
