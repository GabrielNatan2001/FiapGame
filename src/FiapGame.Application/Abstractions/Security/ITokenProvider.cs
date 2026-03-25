using FiapGame.Domain.Usuario.Entities;

namespace FiapGame.Application.Abstractions.Security;

public interface ITokenProvider
{
    string GerarToken(UsuarioEntity usuario);
}
