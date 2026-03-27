using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FiapGame.Application.Jogo.Services;
using FiapGame.Domain.Jogo.Entities;
using FiapGame.Domain.Jogo.Interfaces;
using Moq;
using Xunit;

namespace FiapGame.Application.Tests.Jogo.Services;

public class ListarBibliotecaServiceTests
{
    private readonly Mock<IUsuarioJogoRepository> _usuarioJogoRepositoryMock;
    private readonly ListarBibliotecaService _sut;

    public ListarBibliotecaServiceTests()
    {
        _usuarioJogoRepositoryMock = new Mock<IUsuarioJogoRepository>();
        _sut = new ListarBibliotecaService(_usuarioJogoRepositoryMock.Object);
    }

    [Fact(DisplayName = "Listar Biblioteca Retorna Lista De Jogos Do Usuario")]
    [Trait("Categoria", "Application - ListarBibliotecaService")]
    public async Task ListarBiblioteca_DeveRetornarJogosDoUsuario()
    {
        // Arrange
        var usuarioId = Guid.NewGuid();
        var jogosEntity = new List<JogoEntity>
        {
            JogoEntity.Criar("Jogo 1", "Descricao 1", 10.0m),
            JogoEntity.Criar("Jogo 2", "Descricao 2", 20.0m)
        };

        _usuarioJogoRepositoryMock.Setup(x => x.ObterBiblioteca(usuarioId)).ReturnsAsync(jogosEntity);

        // Act
        var result = await _sut.Execute(usuarioId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Contains(result, j => j.Titulo == "Jogo 1");
        Assert.Contains(result, j => j.Titulo == "Jogo 2");
    }
}
