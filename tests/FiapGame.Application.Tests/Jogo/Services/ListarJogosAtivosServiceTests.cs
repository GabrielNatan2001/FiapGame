using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FiapGame.Application.Jogo.Services;
using FiapGame.Domain.Common.Enums;
using FiapGame.Domain.Jogo.Entities;
using FiapGame.Domain.Jogo.Interfaces;
using Moq;
using Xunit;

namespace FiapGame.Application.Tests.Jogo.Services;

public class ListarJogosAtivosServiceTests
{
    private readonly Mock<IJogoRepository> _jogoRepositoryMock;
    private readonly ListarJogosAtivosService _sut;

    public ListarJogosAtivosServiceTests()
    {
        _jogoRepositoryMock = new Mock<IJogoRepository>();
        _sut = new ListarJogosAtivosService(_jogoRepositoryMock.Object);
    }

    [Fact(DisplayName = "Listar Jogos Ativos Retorna Apenas Jogos Com Status Ativo")]
    public async Task ListarJogosAtivos_DeveRetornarApenasJogosAtivos()
    {
        // Arrange
        var jogoAtivo = JogoEntity.Criar("Ativo", "D", 10, "C");
        var jogoInativo = JogoEntity.Criar("Inativo", "D", 10, "C");
        jogoInativo.AlterarStatus(); // Inativo

        var list = new List<JogoEntity> { jogoAtivo, jogoInativo };
        _jogoRepositoryMock.Setup(x => x.ObterTodos()).ReturnsAsync(list);

        // Act
        var result = await _sut.Execute();

        // Assert
        Assert.Single(result);
        Assert.Equal("Ativo", result.First().Nome);
        Assert.Equal(EStatus.Ativo, result.First().Status);
    }
}
