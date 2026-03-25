using FiapGame.Domain.Jogo.Entities;
using FiapGame.Domain.Usuario.Entities;
using FiapGame.Shared.Exceptions;
using Xunit;

namespace FiapGame.Domain.Tests;

public class UsuarioTests
{
    [Fact]
    public void CriarUsuario_DeveCriarComRoleUser()
    {
        var usuario = UsuarioEntity.Criar("Gabriel", "gabriel@fiap.com", "Senha@123");
        Assert.Equal("User", usuario.Role.ToString());
    }

    [Fact]
    public void CriarJogo_ComPrecoNegativo_DeveLancarExcecao()
    {
        Assert.Throws<DomainException>(() =>
            JogoEntity.Criar("Game Test", "Descricao", -1));
    }
}
