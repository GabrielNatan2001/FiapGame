using System;
using System.Threading.Tasks;
using FiapGame.Application.Abstractions.Security;
using FiapGame.Application.Usuario.Dtos;
using FiapGame.Application.Usuario.Services;
using FiapGame.Domain.Usuario.Entities;
using FiapGame.Domain.Usuario.Interfaces;
using FiapGame.Shared.Exceptions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace FiapGame.Application.Tests.Usuario.Services;

public class AutenticarUsuarioServiceTests
{
    private readonly Mock<IUsuarioRepository> _usuarioRepositoryMock;
    private readonly Mock<ITokenProvider> _tokenProviderMock;
    private readonly Mock<ILogger<AutenticarUsuarioService>> _loggerMock;
    private readonly AutenticarUsuarioService _sut;

    public AutenticarUsuarioServiceTests()
    {
        _usuarioRepositoryMock = new Mock<IUsuarioRepository>();
        _tokenProviderMock = new Mock<ITokenProvider>();
        _loggerMock = new Mock<ILogger<AutenticarUsuarioService>>();
        _sut = new AutenticarUsuarioService(_usuarioRepositoryMock.Object, _tokenProviderMock.Object, _loggerMock.Object);
    }

    [Fact(DisplayName = "Autenticar Usuario Lanca Excecao Quando Email Nao Encontrado")]
    [Trait("Categoria", "Application - AutenticarUsuarioService")]
    public async Task AutenticarUsuario_DeveLancarExcecao_QuandoNaoEncontrarEmail()
    {
        // Arrange
        var request = new AutenticarUsuarioDto.Request { Email = "teste@teste.com", Senha = "123" };
        _usuarioRepositoryMock.Setup(x => x.ObterPorEmailAsync(request.Email)).ReturnsAsync((UsuarioEntity?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<DomainException>(() => _sut.Execute(request));
        Assert.Equal("Email ou senha inválidos.", exception.Message);
        _tokenProviderMock.Verify(x => x.GerarToken(It.IsAny<UsuarioEntity>()), Times.Never);
    }

    [Fact(DisplayName = "Autenticar Usuario Lanca Excecao Quando Senha Invalida")]
    [Trait("Categoria", "Application - AutenticarUsuarioService")]
    public async Task AutenticarUsuario_DeveLancarExcecao_QuandoSenhaEstiverIncorreta()
    {
        // Arrange
        var request = new AutenticarUsuarioDto.Request { Email = "teste@teste.com", Senha = "senhaErrada" };
        var usuario = UsuarioEntity.Criar("Teste", "teste@teste.com", "senhaCorreta");
        
        _usuarioRepositoryMock.Setup(x => x.ObterPorEmailAsync(request.Email)).ReturnsAsync(usuario);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<DomainException>(() => _sut.Execute(request));
        Assert.Equal("Email ou senha inválidos.", exception.Message);
        _tokenProviderMock.Verify(x => x.GerarToken(It.IsAny<UsuarioEntity>()), Times.Never);
    }

    [Fact(DisplayName = "Autenticar Usuario Retorna Token Quando Credenciais Validas")]
    [Trait("Categoria", "Application - AutenticarUsuarioService")]
    public async Task AutenticarUsuario_DeveRetornarToken_QuandoValido()
    {
        // Arrange
        var request = new AutenticarUsuarioDto.Request { Email = "teste@teste.com", Senha = "senhaCorreta" };
        var usuario = UsuarioEntity.Criar("Teste", "teste@teste.com", "senhaCorreta");
        
        _usuarioRepositoryMock.Setup(x => x.ObterPorEmailAsync(request.Email)).ReturnsAsync(usuario);
        _tokenProviderMock.Setup(x => x.GerarToken(usuario)).Returns("my-jwt-token");

        // Act
        var result = await _sut.Execute(request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("my-jwt-token", result.AccessToken);
        Assert.True(result.ExpiraEmUtc > DateTime.UtcNow);
    }
}
