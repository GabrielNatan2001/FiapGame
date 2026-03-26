using System;
using FiapGame.Domain.Usuario.ValuesObjects;
using FiapGame.Shared.Exceptions;
using Xunit;

namespace FiapGame.Domain.Tests.Usuario.ValuesObjects;

public class SenhaVOTests
{
    [Fact(DisplayName = "Criar Senha Valida Deve Gerar Hash")]
    [Trait("Categoria", "Domain - ValueObjects")]
    public void CriarSenha_SenhaValida_DeveGerarHashEValidar()
    {
        // Arrange
        var plainPassword = "Password@123";
        
        // Act
        var senhaVo = SenhaVO.Create(plainPassword);
        
        // Assert
        Assert.NotNull(senhaVo.Hash);
        Assert.True(senhaVo.Verify(plainPassword));
        Assert.False(senhaVo.Verify("Password@124"));
    }
    
    [Theory(DisplayName = "Criar Senha Invalida Deve Lancar Excecao")]
    [Trait("Categoria", "Domain - ValueObjects")]
    [InlineData("")]
    [InlineData("1234567")] // Menos de 8 char
    [InlineData("abcdefgh")] // Sem numero e especial
    [InlineData("12345678")] // Sem letra e especial
    [InlineData("abcdef12")] // Sem especial
    [InlineData("abcdef@#")] // Sem numero
    public void CriarSenha_ConfiguracaoInvalida_DeveLancarExcecao(string senhaInvalida)
    {
        var ex = Assert.Throws<DomainException>(() => SenhaVO.Create(senhaInvalida));
        Assert.Equal("Senha deve ter no mínimo 8 caracteres com letras, números e especiais", ex.Message);
    }
}
