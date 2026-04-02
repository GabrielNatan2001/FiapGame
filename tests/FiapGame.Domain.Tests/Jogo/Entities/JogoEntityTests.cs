using System;
using FiapGame.Domain.Common.Enums;
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
        var nome = "The Witcher 3";
        var descricao = "Um ótimo RPG de ação";
        var preco = 150.0m;
        var categoria = "RPG";

        // Act
        var jogo = JogoEntity.Criar(nome, descricao, preco, categoria);

        // Assert
        Assert.NotNull(jogo);
        Assert.Equal(nome, jogo.Nome);
        Assert.Equal(descricao, jogo.Descricao);
        Assert.Equal(preco, jogo.Preco);
        Assert.Equal(categoria, jogo.Categoria);
        Assert.Equal(EStatus.Ativo, jogo.Status);
    }

    [Theory(DisplayName = "Criar Jogo Com Nome Invalido Deve Lancar Excecao")]
    [Trait("Categoria", "Domain - JogoEntity")]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void CriarJogo_ComNomeInvalido_DeveLancarDomainException(string nomeInvalido)
    {
        // Arrange
        var descricao = "Uma descrição genérica";
        var preco = 100.0m;
        var categoria = "Ação";

        // Act & Assert
        var ex = Assert.Throws<DomainException>(() => JogoEntity.Criar(nomeInvalido, descricao, preco, categoria));
        Assert.Equal("Nome do jogo é obrigatório.", ex.Message);
    }

    [Fact(DisplayName = "Criar Jogo Com Preco Negativo Deve Lancar Excecao")]
    [Trait("Categoria", "Domain - JogoEntity")]
    public void CriarJogo_ComPrecoNegativo_DeveLancarDomainException()
    {
        // Arrange
        var nome = "Cyberpunk 2077";
        var descricao = "Um jogo futurista";
        var precoNegativo = -10.0m;
        var categoria = "Ação";

        // Act & Assert
        var ex = Assert.Throws<DomainException>(() => JogoEntity.Criar(nome, descricao, precoNegativo, categoria));
        Assert.Equal("Preço do jogo não pode ser negativo.", ex.Message);
    }

    [Theory(DisplayName = "Criar Jogo Com Categoria Invalida Deve Lancar Excecao")]
    [Trait("Categoria", "Domain - JogoEntity")]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void CriarJogo_ComCategoriaInvalida_DeveLancarDomainException(string categoriaInvalida)
    {
        // Arrange
        var nome = "The Witcher 3";
        var descricao = "Um ótimo RPG de ação";
        var preco = 150.0m;

        // Act & Assert
        var ex = Assert.Throws<DomainException>(() => JogoEntity.Criar(nome, descricao, preco, categoriaInvalida));
        Assert.Equal("Categoria do jogo é obrigatória.", ex.Message);
    }

    [Fact(DisplayName = "Criar Jogo Com Descricao Nula Deve Retornar Entidade Com Descricao Vazia")]
    [Trait("Categoria", "Domain - JogoEntity")]
    public void CriarJogo_ComDescricaoNula_DeveRetornarEntidadeDescricaoVazia()
    {
        // Arrange
        var nome = "Red Dead Redemption 2";
        string descricaoNula = null;
        var preco = 200.0m;
        var categoria = "Ação";

        // Act
        var jogo = JogoEntity.Criar(nome, descricaoNula, preco, categoria);

        // Assert
        Assert.NotNull(jogo);
        Assert.Equal(nome, jogo.Nome);
        Assert.Equal(string.Empty, jogo.Descricao);
        Assert.Equal(preco, jogo.Preco);
    }
}
