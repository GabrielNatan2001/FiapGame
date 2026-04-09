using System;
using System.Threading.Tasks;
using FiapGame.Application.Jogo.Services;
using FiapGame.Domain.Biblioteca.Entities;
using FiapGame.Domain.Biblioteca.Interfaces;
using FiapGame.Domain.Jogo.Entities;
using FiapGame.Domain.Jogo.Interfaces;
using FiapGame.Shared.Exceptions;
using Moq;
using Xunit;

namespace FiapGame.Application.Tests.Jogo.Services;

public class AdquirirJogoServiceTests
{
    private readonly Mock<IJogoRepository> _jogoRepositoryMock;
    private readonly Mock<IBibliotecaRepository> _bibliotecaRepositoryMock;
    private readonly AdquirirJogoService _sut;

    public AdquirirJogoServiceTests()
    {
        _jogoRepositoryMock = new Mock<IJogoRepository>();
        _bibliotecaRepositoryMock = new Mock<IBibliotecaRepository>();
        _sut = new AdquirirJogoService(_jogoRepositoryMock.Object, _bibliotecaRepositoryMock.Object);
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

        _bibliotecaRepositoryMock.Verify(x => x.SalvarAlteracoes(), Times.Never);
    }

    [Fact(DisplayName = "Adquirir Jogo Emite Excecao Quando Usuario Ja Possui Jogo")]
    [Trait("Categoria", "Application - AdquirirJogoService")]
    public async Task AdquirirJogo_DeveLancarExcecao_QuandoUsuarioJaPossuiOJogo()
    {
        // Arrange
        var usuarioId = Guid.NewGuid();
        var jogoId = Guid.NewGuid();
        var jogo = JogoEntity.Criar("Jogo Existente", "Descricao", 100, "Ação");
        var biblioteca = BibliotecaEntity.Criar(usuarioId);
        biblioteca.AdicionarJogo(jogoId);

        _jogoRepositoryMock.Setup(x => x.ObterPorId(jogoId)).ReturnsAsync(jogo);
        _bibliotecaRepositoryMock.Setup(x => x.ObterPorUsuarioId(usuarioId)).ReturnsAsync(biblioteca);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<DomainException>(() => _sut.Execute(usuarioId, jogoId));
        Assert.Equal("Este jogo já está na sua biblioteca.", exception.Message);

        _bibliotecaRepositoryMock.Verify(x => x.SalvarAlteracoes(), Times.Never);
    }

    [Fact(DisplayName = "Adquirir Jogo Emite Excecao Quando Jogo Esta Inativo")]
    [Trait("Categoria", "Application - AdquirirJogoService")]
    public async Task AdquirirJogo_DeveLancarExcecao_QuandoJogoEstiverInativo()
    {
        // Arrange
        var usuarioId = Guid.NewGuid();
        var jogoId = Guid.NewGuid();
        var jogo = JogoEntity.Criar("Jogo Inativo", "Descricao", 100, "Ação");
        jogo.Desativar(); // Inativo

        _jogoRepositoryMock.Setup(x => x.ObterPorId(jogoId)).ReturnsAsync(jogo);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<DomainException>(() => _sut.Execute(usuarioId, jogoId));
        Assert.Equal("Este jogo não está disponível para aquisição.", exception.Message);

        _bibliotecaRepositoryMock.Verify(x => x.SalvarAlteracoes(), Times.Never);
    }

    [Fact(DisplayName = "Adquirir Jogo Processa Com Sucesso Quando Valido")]
    [Trait("Categoria", "Application - AdquirirJogoService")]
    public async Task AdquirirJogo_DeveAdicionarESalvar_QuandoValido()
    {
        // Arrange
        var usuarioId = Guid.NewGuid();
        var jogoId = Guid.NewGuid();
        var jogo = JogoEntity.Criar("Novo Jogo", "Descricao", 50, "Ação");
        var biblioteca = BibliotecaEntity.Criar(usuarioId);

        _jogoRepositoryMock.Setup(x => x.ObterPorId(jogoId)).ReturnsAsync(jogo);
        _bibliotecaRepositoryMock.Setup(x => x.ObterPorUsuarioId(usuarioId)).ReturnsAsync(biblioteca);
        _bibliotecaRepositoryMock.Setup(x => x.SalvarAlteracoes()).Returns(Task.CompletedTask);

        // Act
        await _sut.Execute(usuarioId, jogoId);

        // Assert
        Assert.Contains(biblioteca.Itens, x => x.JogoId == jogoId);
        _bibliotecaRepositoryMock.Verify(x => x.SalvarAlteracoes(), Times.Once);
    }

    [Fact(DisplayName = "Adquirir Jogo Cria Biblioteca Quando Nao Existe")]
    [Trait("Categoria", "Application - AdquirirJogoService")]
    public async Task AdquirirJogo_DeveCriarBiblioteca_QuandoUsuarioNaoTiverUma()
    {
        // Arrange
        var usuarioId = Guid.NewGuid();
        var jogoId = Guid.NewGuid();
        var jogo = JogoEntity.Criar("Novo Jogo", "Descricao", 50, "Ação");

        _jogoRepositoryMock.Setup(x => x.ObterPorId(jogoId)).ReturnsAsync(jogo);
        _bibliotecaRepositoryMock.Setup(x => x.ObterPorUsuarioId(usuarioId)).ReturnsAsync((BibliotecaEntity?)null);
        _bibliotecaRepositoryMock.Setup(x => x.Adicionar(It.IsAny<BibliotecaEntity>())).Returns(Task.CompletedTask);
        _bibliotecaRepositoryMock.Setup(x => x.SalvarAlteracoes()).Returns(Task.CompletedTask);

        // Act
        await _sut.Execute(usuarioId, jogoId);

        // Assert
        _bibliotecaRepositoryMock.Verify(x => x.Adicionar(It.Is<BibliotecaEntity>(b => b.UsuarioId == usuarioId)), Times.Once);
        _bibliotecaRepositoryMock.Verify(x => x.SalvarAlteracoes(), Times.Once);
    }
}
