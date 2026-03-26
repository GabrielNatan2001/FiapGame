using System;
using FiapGame.Domain.Usuario.ValuesObjects;
using FiapGame.Shared.Exceptions;
using Xunit;

namespace FiapGame.Domain.Tests.Usuario.ValuesObjects;

public class EmailVoTests
{
    [Fact(DisplayName = "Criar Email Valido Deve Retornar VO Com Valor Definido")]
    [Trait("Categoria", "Domain - ValueObjects")]
    public void CriarEmail_EmailValido_DeveRetornarInstancia()
    {
        // Arrange
        var emailString = "teste@fiap.com.br";
        
        // Act
        var emailVo = new EmailVo(emailString);
        
        // Assert
        Assert.Equal("teste@fiap.com.br", emailVo.Value);
    }
    
    [Theory(DisplayName = "Criar Email Invalido Deve Lancar Excecao")]
    [Trait("Categoria", "Domain - ValueObjects")]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void CriarEmail_EmailNuloOuVazio_DeveLancarExcecao(string emailInvalido)
    {
        var ex = Assert.Throws<DomainException>(() => new EmailVo(emailInvalido));
        Assert.Equal("Email não pode ser vazio", ex.Message);
    }

    [Theory(DisplayName = "Criar Email Com Formato Invalido Deve Lancar Excecao")]
    [Trait("Categoria", "Domain - ValueObjects")]
    [InlineData("testefiap.com")]
    [InlineData("teste@")]
    [InlineData("teste@fiap")]
    [InlineData("@fiap.com")]
    public void CriarEmail_FormatoInvalido_DeveLancarExcecao(string emailInvalido)
    {
        var ex = Assert.Throws<DomainException>(() => new EmailVo(emailInvalido));
        Assert.Equal("Email inválido", ex.Message);
    }
}
