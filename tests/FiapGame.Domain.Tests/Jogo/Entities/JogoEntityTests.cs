using System;
using FiapGame.Domain.Jogo.Entities;
using FiapGame.Shared.Exceptions;
using Xunit;

namespace FiapGame.Domain.Tests.Jogo.Entities;

public class JogoEntityTests
{
    [Fact(DisplayName = "Criar Jogo Valido Deve Retornar Entidade")]
    [Trait("Categoria", "Domain - JogoEntity")]
    public void CriarJogo_ComParametrosValidos_DeveRetornarInstanciaValida()
    {
        // Arrange
        var titulo = "The Witcher 3";
        var descricao = "Um ótimo RPG de ação";
        var preco = 150.0m;

        // Act
        var jogo = JogoEntity.Criar(titulo, descricao, preco);

        // Assert
        Assert.NotNull(jogo);
        Assert.Equal(titulo, jogo.Titulo);
        Assert.Equal(descricao, jogo.Descricao);
        Assert.Equal(preco, jogo.Preco);
    }

    [Theory(DisplayName = "Criar Jogo Com Titulo Invalido Deve Lancar Excecao")]
    [Trait("Categoria", "Domain - JogoEntity")]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void CriarJogo_ComTituloInvalido_DeveLancarDomainException(string tituloInvalido)
    {
        // Arrange
        var descricao = "Uma descrição genérica";
        var preco = 100.0m;

        // Act & Assert
        var ex = Assert.Throws<DomainException>(() => JogoEntity.Criar(tituloInvalido, descricao, preco));
        Assert.Equal("Titulo do jogo é obrigatório.", ex.Message);
    }

    [Fact(DisplayName = "Criar Jogo Com Preco Negativo Deve Lancar Excecao")]
    [Trait("Categoria", "Domain - JogoEntity")]
    public void CriarJogo_ComPrecoNegativo_DeveLancarDomainException()
    {
        // Arrange
        var titulo = "Cyberpunk 2077";
        var descricao = "Um jogo futurista";
        var precoNegativo = -10.0m;

        // Act & Assert
        var ex = Assert.Throws<DomainException>(() => JogoEntity.Criar(titulo, descricao, precoNegativo));
        Assert.Equal("Preço do jogo não pode ser negativo.", ex.Message);
    }

    [Fact(DisplayName = "Criar Jogo Com Descricao Nula Deve Retornar Entidade Com Descricao Vazia")]
    [Trait("Categoria", "Domain - JogoEntity")]
    public void CriarJogo_ComDescricaoNula_DeveRetornarEntidadeDescricaoVazia()
    {
        // Arrange
        var titulo = "Red Dead Redemption 2";
        string descricaoNula = null;
        var preco = 200.0m;

        // Act
        var jogo = JogoEntity.Criar(titulo, descricaoNula, preco);

        // Assert
        Assert.NotNull(jogo);
        Assert.Equal(titulo, jogo.Titulo);
        Assert.Equal(string.Empty, jogo.Descricao);
        Assert.Equal(preco, jogo.Preco);
    }
}
