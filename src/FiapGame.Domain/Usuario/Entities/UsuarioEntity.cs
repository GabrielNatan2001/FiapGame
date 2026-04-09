using FiapGame.Domain.Common.Enums;
using FiapGame.Domain.Usuario.Enum;
using FiapGame.Domain.Usuario.ValuesObjects;
using FiapGame.Shared.Base;
using FiapGame.Shared.Exceptions;

namespace FiapGame.Domain.Usuario.Entities;

public class UsuarioEntity : BaseEntity
{
    public string Nome { get; private set; }
    public EmailVo Email { get; private set; }
    public SenhaVO Password { get; private set; }
    public EPerfil Role { get; private set; }
    public EStatus Status { get; private set; }

    protected UsuarioEntity() { } // EF Core

    private UsuarioEntity(string nome, EmailVo email, SenhaVO password, EPerfil role, EStatus status)
    {
        Nome = nome;
        Email = email;
        Password = password;
        Role = role;
        Status = status;
    }

    public static UsuarioEntity Criar(string nome, string email, string senha)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new DomainException("Nome é obrigatório");

        var emailVO = new EmailVo(email);
        var passwordVO = SenhaVO.Create(senha);

        return new UsuarioEntity(nome, emailVO, passwordVO, EPerfil.User, EStatus.Ativo);
    }

    public static UsuarioEntity CriarAdmin(string nome, string email, string senha)
    {
        var usuario = Criar(nome, email, senha);
        usuario.Role = EPerfil.Admin;

        return usuario;
    }

    public void Ativar()
    {
        Status = EStatus.Ativo;
    }

    public void Inativar()
    {
        Status = EStatus.Inativo;
    }
}
