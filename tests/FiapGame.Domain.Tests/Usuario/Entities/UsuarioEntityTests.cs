using System;
using FiapGame.Domain.Usuario.Entities;
using FiapGame.Domain.Usuario.Enum;
using FiapGame.Shared.Exceptions;
using Xunit;

namespace FiapGame.Domain.Tests.Usuario.Entities;

public class UsuarioEntityTests
{
    [Fact(DisplayName = "Criar Usuario Valido Deve Retornar Entidade Atribuida a Role User")]
    [Trait("Categoria", "Domain - UsuarioEntity")]
    public void CriarUsuario_ComDadosValidos_DeveRetornarEntidadeComRoleUser()
    {
        // Arrange
        var nome = "Gabriel Natan";
        var email = "gabriel@fiap.com";
        var senha = "Password@123";

        // Act
        var usuario = UsuarioEntity.Criar(nome, email, senha);

        // Assert
        Assert.NotNull(usuario);
        Assert.Equal(nome, usuario.Nome);
        Assert.Equal(email, usuario.Email.Value); // Value do EmailVo
        Assert.True(usuario.Password.Verify(senha)); // Value do SenhaVO
        Assert.Equal(EPerfil.User, usuario.Role);
    }

    [Fact(DisplayName = "Criar Admin Valido Deve Retornar Entidade Atribuida a Role Admin")]
    [Trait("Categoria", "Domain - UsuarioEntity")]
    public void CriarAdmin_ComDadosValidos_DeveRetornarEntidadeComRoleAdmin()
    {
        // Arrange
        var nome = "Admin Natan";
        var email = "admin@fiap.com";
        var senha = "AdminPassword@123";

        // Act
        var usuario = UsuarioEntity.CriarAdmin(nome, email, senha);

        // Assert
        Assert.NotNull(usuario);
        Assert.Equal(nome, usuario.Nome);
        Assert.Equal(EPerfil.Admin, usuario.Role);
    }

    [Theory(DisplayName = "Criar Usuario Sem Nome Deve Lancar Excecao")]
    [Trait("Categoria", "Domain - UsuarioEntity")]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void CriarUsuario_ComNomeInvalido_DeveLancarExcecao(string nomeInvalido)
    {
        // Arrange
        var email = "gabriel@fiap.com";
        var senha = "Password@123";

        // Act & Assert
        var ex = Assert.Throws<DomainException>(() => UsuarioEntity.Criar(nomeInvalido, email, senha));
        Assert.Equal("Nome é obrigatório", ex.Message);
    }
}
