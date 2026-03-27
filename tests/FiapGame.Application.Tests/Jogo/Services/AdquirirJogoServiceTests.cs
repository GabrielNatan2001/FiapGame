using System;
using System.Threading.Tasks;
using FiapGame.Application.Jogo.Services;
using FiapGame.Domain.Jogo.Entities;
using FiapGame.Domain.Jogo.Interfaces;
using FiapGame.Shared.Exceptions;
using Moq;
using Xunit;

namespace FiapGame.Application.Tests.Jogo.Services;

public class AdquirirJogoServiceTests
{
    private readonly Mock<IJogoRepository> _jogoRepositoryMock;
    private readonly Mock<IUsuarioJogoRepository> _usuarioJogoRepositoryMock;
    private readonly AdquirirJogoService _sut;

    public AdquirirJogoServiceTests()
    {
        _jogoRepositoryMock = new Mock<IJogoRepository>();
        _usuarioJogoRepositoryMock = new Mock<IUsuarioJogoRepository>();
        _sut = new AdquirirJogoService(_jogoRepositoryMock.Object, _usuarioJogoRepositoryMock.Object);
    }

    [Fact(DisplayName = "Adquirir Jogo Emite Excecao Quando Jogo Nao Existe")]
    [Trait("Categoria", "Application - AdquirirJogoService")]
    public async Task AdquirirJogo_DeveLancarExcecao_QuandoJogoNaoEncontrado()
    {
        // Arrange
        var usuarioId = Guid.NewGuid();
        var jogoId = Guid.NewGuid();

        _jogoRepositoryMock.Setup(x => x.ObterPorId(jogoId)).ReturnsAsync((JogoEntity?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<DomainException>(() => _sut.Execute(usuarioId, jogoId));
        Assert.Equal("Jogo não encontrado.", exception.Message);

        _usuarioJogoRepositoryMock.Verify(x => x.Adicionar(It.IsAny<UsuarioJogoEntity>()), Times.Never);
        _usuarioJogoRepositoryMock.Verify(x => x.SalvarAlteracoes(), Times.Never);
    }

    [Fact(DisplayName = "Adquirir Jogo Emite Excecao Quando Usuario Ja Possui Jogo")]
    [Trait("Categoria", "Application - AdquirirJogoService")]
    public async Task AdquirirJogo_DeveLancarExcecao_QuandoUsuarioJaPossuiOJogo()
    {
        // Arrange
        var usuarioId = Guid.NewGuid();
        var jogoId = Guid.NewGuid();
        var jogo = JogoEntity.Criar("Jogo Existente", "Descricao", 100);

        _jogoRepositoryMock.Setup(x => x.ObterPorId(jogoId)).ReturnsAsync(jogo);
        _usuarioJogoRepositoryMock.Setup(x => x.UsuarioPossuiJogo(usuarioId, jogoId)).ReturnsAsync(true);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<DomainException>(() => _sut.Execute(usuarioId, jogoId));
        Assert.Equal("Jogo já adquirido para este usuário.", exception.Message);

        _usuarioJogoRepositoryMock.Verify(x => x.Adicionar(It.IsAny<UsuarioJogoEntity>()), Times.Never);
        _usuarioJogoRepositoryMock.Verify(x => x.SalvarAlteracoes(), Times.Never);
    }

    [Fact(DisplayName = "Adquirir Jogo Processa Com Sucesso Quando Valido")]
    [Trait("Categoria", "Application - AdquirirJogoService")]
    public async Task AdquirirJogo_DeveAdicionarESalvar_QuandoValido()
    {
        // Arrange
        var usuarioId = Guid.NewGuid();
        var jogoId = Guid.NewGuid();
        var jogo = JogoEntity.Criar("Novo Jogo", "Descricao", 50);

        _jogoRepositoryMock.Setup(x => x.ObterPorId(jogoId)).ReturnsAsync(jogo);
        _usuarioJogoRepositoryMock.Setup(x => x.UsuarioPossuiJogo(usuarioId, jogoId)).ReturnsAsync(false);
        _usuarioJogoRepositoryMock.Setup(x => x.Adicionar(It.IsAny<UsuarioJogoEntity>())).Returns(Task.CompletedTask);
        _usuarioJogoRepositoryMock.Setup(x => x.SalvarAlteracoes()).Returns(Task.CompletedTask);

        // Act
        await _sut.Execute(usuarioId, jogoId);

        // Assert
        _usuarioJogoRepositoryMock.Verify(x => x.Adicionar(It.Is<UsuarioJogoEntity>(uj => uj.UsuarioId == usuarioId && uj.JogoId == jogoId)), Times.Once);
        _usuarioJogoRepositoryMock.Verify(x => x.SalvarAlteracoes(), Times.Once);
    }
}
