using System;
using System.Threading.Tasks;
using FiapGame.Application.Usuario.Dtos;
using FiapGame.Application.Usuario.Services;
using FiapGame.Domain.Usuario.Entities;
using FiapGame.Domain.Usuario.Interfaces;
using FiapGame.Shared.Exceptions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace FiapGame.Application.Tests.Usuario.Services;

public class CriarUsuarioServiceTests
{
    private readonly Mock<IUsuarioRepository> _usuarioRepositoryMock;
    private readonly Mock<ILogger<CriarUsuarioService>> _loggerMock;
    private readonly CriarUsuarioService _sut;

    public CriarUsuarioServiceTests()
    {
        _usuarioRepositoryMock = new Mock<IUsuarioRepository>();
        _loggerMock = new Mock<ILogger<CriarUsuarioService>>();
        _sut = new CriarUsuarioService(_usuarioRepositoryMock.Object, _loggerMock.Object);
    }

    [Fact(DisplayName = "Criar Usuario Lanca Excecao Quando Email Ja Existe")]
    [Trait("Categoria", "Application - CriarUsuarioService")]
    public async Task CriarUsuario_DeveLancarExcecao_QuandoEmailJaCadastrado()
    {
        // Arrange
        var request = new CriarUsuarioDto.Request { Nome = "Teste", Email = "teste@teste.com", Senha = "123" };
        var usuarioExistente = UsuarioEntity.Criar("Outro", "teste@teste.com", "asd");
        
        _usuarioRepositoryMock.Setup(x => x.ObterPorEmailAsync(request.Email)).ReturnsAsync(usuarioExistente);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<DomainException>(() => _sut.Execute(request));
        Assert.Equal("Email já cadastrado", exception.Message);

        _usuarioRepositoryMock.Verify(x => x.Adicionar(It.IsAny<UsuarioEntity>()), Times.Never);
        _usuarioRepositoryMock.Verify(x => x.SalvarAlteracoes(), Times.Never);
    }

    [Fact(DisplayName = "Criar Usuario Cria Entidade E Salva QuandoEmail Nao Existe")]
    [Trait("Categoria", "Application - CriarUsuarioService")]
    public async Task CriarUsuario_DeveCriarESalvar_QuandoValido()
    {
        // Arrange
        var request = new CriarUsuarioDto.Request { Nome = "Teste", Email = "novo@teste.com", Senha = "123" };
        
        _usuarioRepositoryMock.Setup(x => x.ObterPorEmailAsync(request.Email)).ReturnsAsync((UsuarioEntity?)null);
        _usuarioRepositoryMock.Setup(x => x.Adicionar(It.IsAny<UsuarioEntity>())).Returns(Task.CompletedTask);
        _usuarioRepositoryMock.Setup(x => x.SalvarAlteracoes()).Returns(Task.CompletedTask);

        // Act
        var result = await _sut.Execute(request);

        // Assert
        Assert.NotEqual(Guid.Empty, result);
        _usuarioRepositoryMock.Verify(x => x.Adicionar(It.Is<UsuarioEntity>(u => u.Nome == request.Nome && u.Email == request.Email)), Times.Once);
        _usuarioRepositoryMock.Verify(x => x.SalvarAlteracoes(), Times.Once);
    }
}
