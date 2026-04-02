using System;
using System.Threading.Tasks;
using FiapGame.Application.Jogo.Dtos;
using FiapGame.Application.Jogo.Services;
using FiapGame.Domain.Jogo.Entities;
using FiapGame.Domain.Jogo.Interfaces;
using FiapGame.Shared.Exceptions;
using Moq;
using Xunit;

namespace FiapGame.Application.Tests.Jogo.Services;

public class AtualizarJogoServiceTests
{
    private readonly Mock<IJogoRepository> _jogoRepositoryMock;
    private readonly AtualizarJogoService _sut;

    public AtualizarJogoServiceTests()
    {
        _jogoRepositoryMock = new Mock<IJogoRepository>();
        _sut = new AtualizarJogoService(_jogoRepositoryMock.Object);
    }

    [Fact(DisplayName = "Atualizar Jogo Lanca Excecao Quando Jogo Nao Encontrado")]
    public async Task AtualizarJogo_DeveLancarExcecao_QuandoJogoNaoExiste()
    {
        // Arrange
        var id = Guid.NewGuid();
        var request = new AtualizarJogoDto.Request { Nome = "Novo", Descricao = "D", Preco = 10, Categoria = "C" };
        _jogoRepositoryMock.Setup(x => x.ObterPorId(id)).ReturnsAsync((JogoEntity?)null);

        // Act & Assert
        await Assert.ThrowsAsync<DomainException>(() => _sut.Execute(id, request));
    }

    [Fact(DisplayName = "Atualizar Jogo Processa Com Sucesso Quando Valido")]
    public async Task AtualizarJogo_DeveAtualizarESalvar_QuandoValido()
    {
        // Arrange
        var id = Guid.NewGuid();
        var request = new AtualizarJogoDto.Request 
        { 
            Nome = "Nome Atualizado", 
            Descricao = "Nova Descricao", 
            Preco = 200, 
            Categoria = "Nova Categoria" 
        };
        var jogo = JogoEntity.Criar("Antigo", "Antiga", 100, "Antiga");

        _jogoRepositoryMock.Setup(x => x.ObterPorId(id)).ReturnsAsync(jogo);

        // Act
        await _sut.Execute(id, request);

        // Assert
        Assert.Equal(request.Nome, jogo.Nome);
        Assert.Equal(request.Descricao, jogo.Descricao);
        Assert.Equal(request.Preco, jogo.Preco);
        Assert.Equal(request.Categoria, jogo.Categoria);

        _jogoRepositoryMock.Verify(x => x.Atualizar(jogo), Times.Once);
        _jogoRepositoryMock.Verify(x => x.SalvarAlteracoes(), Times.Once);
    }
}
