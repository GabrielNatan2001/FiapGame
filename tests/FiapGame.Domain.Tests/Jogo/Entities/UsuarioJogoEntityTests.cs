using System;
using FiapGame.Domain.Jogo.Entities;
using Xunit;

namespace FiapGame.Domain.Tests.Jogo.Entities;

public class UsuarioJogoEntityTests
{
    [Fact(DisplayName = "Criar UsuarioJogo Retorna Entidade Com IDs Corretos")]
    [Trait("Categoria", "Domain - UsuarioJogoEntity")]
    public void CriarUsuarioJogo_DeveAtribuirPropriedadesCorretamente()
    {
        // Arrange
        var usuarioId = Guid.NewGuid();
        var jogoId = Guid.NewGuid();

        // Act
        var usuarioJogo = UsuarioJogoEntity.Criar(usuarioId, jogoId);

        // Assert
        Assert.NotNull(usuarioJogo);
        Assert.Equal(usuarioId, usuarioJogo.UsuarioId);
        Assert.Equal(jogoId, usuarioJogo.JogoId);
    }
}
