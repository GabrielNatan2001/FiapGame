using System;
using System.Threading.Tasks;
using FiapGame.Application.Jogo.Dtos;
using FiapGame.Application.Jogo.Services;
using FiapGame.Domain.Jogo.Entities;
using FiapGame.Domain.Jogo.Interfaces;
using Moq;
using Xunit;

namespace FiapGame.Application.Tests.Jogo.Services;

public class CriarJogoServiceTests
{
    private readonly Mock<IJogoRepository> _jogoRepositoryMock;
    private readonly CriarJogoService _sut;

    public CriarJogoServiceTests()
    {
        _jogoRepositoryMock = new Mock<IJogoRepository>();
        _sut = new CriarJogoService(_jogoRepositoryMock.Object);
    }

    [Fact(DisplayName = "Criar Jogo Deve Cadastrar E Retornar Id Quando Dados Validos")]
    [Trait("Categoria", "Application - CriarJogoService")]
    public async Task CriarJogo_DeveAdicionarESalvar_QuandoDadosForemValidos()
    {
        // Arrange
        var request = new CriarJogoDto.Request
        {
            Nome = "Novo Jogo",
            Descricao = "Descrição do jogo",
            Preco = 150.0m,
            Categoria = "Ação"
        };

        _jogoRepositoryMock.Setup(x => x.Adicionar(It.IsAny<JogoEntity>())).Returns(Task.CompletedTask);
        _jogoRepositoryMock.Setup(x => x.SalvarAlteracoes()).ReturnsAsync(1);

        // Act
        var result = await _sut.Execute(request);

        // Assert
        Assert.NotEqual(Guid.Empty, result);
        _jogoRepositoryMock.Verify(x => x.Adicionar(It.Is<JogoEntity>(j => 
            j.Nome == request.Nome && 
            j.Descricao == request.Descricao && 
            j.Preco == request.Preco &&
            j.Categoria == request.Categoria)), Times.Once);
        _jogoRepositoryMock.Verify(x => x.SalvarAlteracoes(), Times.Once);
    }
}
