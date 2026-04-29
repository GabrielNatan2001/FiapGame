using FiapGame.Application.Usuario.Dtos;
using FiapGame.Application.Usuario.Services;
using FiapGame.Domain.Usuario.Entities;
using FiapGame.Domain.Usuario.Interfaces;
using FiapGame.Shared.Exceptions;
using Microsoft.Extensions.Logging;
using Moq;
using Reqnroll;
using Xunit;

namespace FiapGame.BDD.Tests.StepDefinitions;

[Binding]
public class CriarUsuarioStepDefinitions
{
    private CriarUsuarioDto.Request? _request;
    private readonly Mock<IUsuarioRepository> _repositoryMock;
    private readonly Mock<ILogger<CriarUsuarioService>> _loggerMock;
    private readonly CriarUsuarioService _service;
    private Guid _usuarioIdResultado;
    private Exception? _exceptionResultado;

    public CriarUsuarioStepDefinitions()
    {
        _repositoryMock = new Mock<IUsuarioRepository>();
        _repositoryMock.Setup(r => r.Adicionar(It.IsAny<UsuarioEntity>()))
            .Callback<UsuarioEntity>(u => 
            {
                var propertyInfo = typeof(FiapGame.Shared.Base.BaseEntity).GetProperty("Id");
                propertyInfo?.SetValue(u, Guid.NewGuid());
            });
        _loggerMock = new Mock<ILogger<CriarUsuarioService>>();
        _service = new CriarUsuarioService(_repositoryMock.Object, _loggerMock.Object);
    }

    [Given(@"que eu informo os seguintes dados para cadastro:")]
    public void DadoQueEuInformoOsSeguintesDadosParaCadastro(DataTable dataTable)
    {
        var row = dataTable.Rows[0];
        _request = new CriarUsuarioDto.Request
        {
            Nome = row["Nome"],
            Email = row["Email"],
            Senha = row["Senha"]
        };
    }

    [Given(@"o e-mail ""(.*)"" não está cadastrado no sistema")]
    public void DadoOEmailNaoEstaCadastradoNoSistema(string email)
    {
        _repositoryMock.Setup(r => r.ObterPorEmailAsync(email))
            .ReturnsAsync((UsuarioEntity?)null);
    }

    [Given(@"o e-mail ""(.*)"" já está cadastrado no sistema")]
    public void DadoOEmailJaEstaCadastradoNoSistema(string email)
    {
        var usuarioExistente = UsuarioEntity.Criar("Outro Nome", email, "Senha@123");
        _repositoryMock.Setup(r => r.ObterPorEmailAsync(email))
            .ReturnsAsync(usuarioExistente);
    }

    [When(@"eu solicitar a criação do usuário")]
    public async Task QuandoEuSolicitarACriacaoDoUsuario()
    {
        try
        {
            _usuarioIdResultado = await _service.Execute(_request!);
        }
        catch (Exception ex)
        {
            _exceptionResultado = ex;
        }
    }

    [Then(@"o usuário deve ser criado com sucesso")]
    public void EntaoOUsuarioDeveSerCriadoComSucesso()
    {
        Assert.Null(_exceptionResultado);
        Assert.NotEqual(Guid.Empty, _usuarioIdResultado);
        
        _repositoryMock.Verify(r => r.Adicionar(It.IsAny<UsuarioEntity>()), Times.Once);
    }

    [Then(@"o repositório deve salvar as alterações")]
    public void EntaoORepositorioDeveSalvarAsAlteracoes()
    {
        _repositoryMock.Verify(r => r.SalvarAlteracoes(), Times.Once);
    }

    [Then(@"o sistema deve retornar um erro de domínio com a mensagem ""(.*)""")]
    public void EntaoOSistemaDeveRetornarUmErroDeDominioComAMensagem(string mensagem)
    {
        Assert.NotNull(_exceptionResultado);
        var domainException = Assert.IsType<DomainException>(_exceptionResultado);
        Assert.Equal(mensagem, domainException.Message);

        _repositoryMock.Verify(r => r.Adicionar(It.IsAny<UsuarioEntity>()), Times.Never);
        _repositoryMock.Verify(r => r.SalvarAlteracoes(), Times.Never);
    }
}
