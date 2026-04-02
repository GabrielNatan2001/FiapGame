using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FiapGame.Application.Jogo.Services;
using FiapGame.Domain.Jogo.Entities;
using FiapGame.Domain.Jogo.Interfaces;
using Moq;
using Xunit;

namespace FiapGame.Application.Tests.Jogo.Services;

public class ListarJogosServiceTests
{
    private readonly Mock<IJogoRepository> _jogoRepositoryMock;
    private readonly ListarJogosService _sut;

    public ListarJogosServiceTests()
    {
        _jogoRepositoryMock = new Mock<IJogoRepository>();
        _sut = new ListarJogosService(_jogoRepositoryMock.Object);
    }

    [Fact(DisplayName = "Listar Jogos Retorna Lista Vazia Quando Nao Existem Jogos")]
    [Trait("Categoria", "Application - ListarJogosService")]
    public async Task ListarJogos_DeveRetornarListaVazia_QuandoNaoHouverJogos()
    {
        // Arrange
        _jogoRepositoryMock.Setup(x => x.ObterTodos()).ReturnsAsync(new List<JogoEntity>());

        // Act
        var result = await _sut.Execute();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact(DisplayName = "Listar Jogos Retorna Lista De Todos Os Jogos")]
    [Trait("Categoria", "Application - ListarJogosService")]
    public async Task ListarJogos_DeveRetornarJogosDisponiveis()
    {
        // Arrange
        var jogosEntity = new List<JogoEntity>
        {
            JogoEntity.Criar("Aventura 1", "Um jogo legal", 199.99m, "Aventura"),
            JogoEntity.Criar("Corrida Turbo", "Alta velocidade", 99.90m, "Corrida")
        };

        _jogoRepositoryMock.Setup(x => x.ObterTodos()).ReturnsAsync(jogosEntity);

        // Act
        var result = await _sut.Execute();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Contains(result, j => j.Nome == "Aventura 1");
        Assert.Contains(result, j => j.Nome == "Corrida Turbo");
    }
}
