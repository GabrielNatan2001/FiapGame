using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FiapGame.Application.Biblioteca.Services;
using FiapGame.Domain.Biblioteca.Interfaces;
using FiapGame.Domain.Jogo.Entities;
using Moq;
using Xunit;

namespace FiapGame.Application.Tests.Biblioteca.Services;

public class ListarBibliotecaServiceTests
{
    private readonly Mock<IBibliotecaRepository> _bibliotecaRepositoryMock;
    private readonly ListarBibliotecaService _sut;

    public ListarBibliotecaServiceTests()
    {
        _bibliotecaRepositoryMock = new Mock<IBibliotecaRepository>();
        _sut = new ListarBibliotecaService(_bibliotecaRepositoryMock.Object);
    }

    [Fact(DisplayName = "Listar Biblioteca Retorna Lista De Jogos Do Usuario")]
    [Trait("Categoria", "Application - ListarBibliotecaService")]
    public async Task ListarBiblioteca_DeveRetornarJogosDoUsuario()
    {
        // Arrange
        var usuarioId = Guid.NewGuid();
        var jogosEntity = new List<JogoEntity>
        {
            JogoEntity.Criar("Jogo 1", "Descricao 1", 10.0m, "Ação"),
            JogoEntity.Criar("Jogo 2", "Descricao 2", 20.0m, "Aventura")
        };

        _bibliotecaRepositoryMock.Setup(x => x.ObterJogosDaBiblioteca(usuarioId)).ReturnsAsync(jogosEntity);

        // Act
        var result = await _sut.Execute(usuarioId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Contains(result, j => j.Nome == "Jogo 1");
        Assert.Contains(result, j => j.Nome == "Jogo 2");
    }
}
