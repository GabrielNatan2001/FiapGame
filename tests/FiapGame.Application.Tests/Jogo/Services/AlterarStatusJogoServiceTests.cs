using System;
using System.Threading.Tasks;
using FiapGame.Application.Jogo.Services;
using FiapGame.Domain.Common.Enums;
using FiapGame.Domain.Jogo.Entities;
using FiapGame.Domain.Jogo.Interfaces;
using FiapGame.Shared.Exceptions;
using Moq;
using Xunit;

namespace FiapGame.Application.Tests.Jogo.Services;

public class AlterarStatusJogoServiceTests
{
    private readonly Mock<IJogoRepository> _jogoRepositoryMock;
    private readonly AlterarStatusJogoService _sut;

    public AlterarStatusJogoServiceTests()
    {
        _jogoRepositoryMock = new Mock<IJogoRepository>();
        _sut = new AlterarStatusJogoService(_jogoRepositoryMock.Object);
    }

    [Fact(DisplayName = "Alterar Status Jogo Lanca Excecao Quando Jogo Nao Encontrado")]
    public async Task AlterarStatusJogo_DeveLancarExcecao_QuandoJogoNaoExiste()
    {
        // Arrange
        var id = Guid.NewGuid();
        _jogoRepositoryMock.Setup(x => x.ObterPorId(id)).ReturnsAsync((JogoEntity?)null);

        // Act & Assert
        await Assert.ThrowsAsync<DomainException>(() => _sut.Execute(id));
    }

    [Fact(DisplayName = "Alterar Status Jogo Alterna Entre Ativo E Inativo")]
    public async Task AlterarStatusJogo_DeveAlternarStatusESalvar()
    {
        // Arrange
        var id = Guid.NewGuid();
        var jogo = JogoEntity.Criar("Nome", "Desc", 100, "Categoria"); // Ativo por padrão

        _jogoRepositoryMock.Setup(x => x.ObterPorId(id)).ReturnsAsync(jogo);

        // Act 1
        await _sut.Execute(id);

        // Assert 1
        Assert.Equal(EStatus.Inativo, jogo.Status);
        _jogoRepositoryMock.Verify(x => x.Atualizar(jogo), Times.Once);

        // Act 2
        await _sut.Execute(id);

        // Assert 2
        Assert.Equal(EStatus.Ativo, jogo.Status);
        _jogoRepositoryMock.Verify(x => x.Atualizar(jogo), Times.Exactly(2));
        _jogoRepositoryMock.Verify(x => x.SalvarAlteracoes(), Times.Exactly(2));
    }
}
