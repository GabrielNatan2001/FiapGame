using System;
using FiapGame.Domain.Biblioteca.Entities;
using FiapGame.Shared.Exceptions;
using Xunit;

namespace FiapGame.Domain.Tests.Biblioteca.Entities;

public class BibliotecaTests
{
    [Fact(DisplayName = "Criar Biblioteca Deve Inicializar Com UsuarioId")]
    [Trait("Categoria", "Domain - Biblioteca")]
    public void Criar_DeveInicializarComUsuarioId()
    {
        // Arrange
        var usuarioId = Guid.NewGuid();

        // Act
        var biblioteca = BibliotecaEntity.Criar(usuarioId);

        // Assert
        Assert.Equal(usuarioId, biblioteca.UsuarioId);
        Assert.Empty(biblioteca.Itens);
    }

    [Fact(DisplayName = "Adicionar Jogo Deve Incluir Item Na Biblioteca")]
    [Trait("Categoria", "Domain - Biblioteca")]
    public void AdicionarJogo_DeveIncluirItemNaBiblioteca()
    {
        // Arrange
        var biblioteca = BibliotecaEntity.Criar(Guid.NewGuid());
        var jogoId = Guid.NewGuid();

        // Act
        biblioteca.AdicionarJogo(jogoId);

        // Assert
        Assert.Single(biblioteca.Itens);
        Assert.Contains(biblioteca.Itens, x => x.JogoId == jogoId);
    }

    [Fact(DisplayName = "Adicionar Jogo Duplicado Deve Lancar Excecao")]
    [Trait("Categoria", "Domain - Biblioteca")]
    public void AdicionarJogo_DeveLancarExcecao_QuandoJogoJaExiste()
    {
        // Arrange
        var biblioteca = BibliotecaEntity.Criar(Guid.NewGuid());
        var jogoId = Guid.NewGuid();
        biblioteca.AdicionarJogo(jogoId);

        // Act & Assert
        var exception = Assert.Throws<DomainException>(() => biblioteca.AdicionarJogo(jogoId));
        Assert.Equal("Este jogo já está na sua biblioteca.", exception.Message);
    }

    [Fact(DisplayName = "Possui Jogo Deve Retornar Verdadeiro Se Jogo Estiver Na Biblioteca")]
    [Trait("Categoria", "Domain - Biblioteca")]
    public void PossuiJogo_DeveRetornarVerdadeiro_SeJogoEstiverNaBiblioteca()
    {
        // Arrange
        var biblioteca = BibliotecaEntity.Criar(Guid.NewGuid());
        var jogoId = Guid.NewGuid();
        biblioteca.AdicionarJogo(jogoId);

        // Act
        var resultado = biblioteca.PossuiJogo(jogoId);

        // Assert
        Assert.True(resultado);
    }
}
