using FiapGame.Domain.Usuario.Entities;
using FiapGame.Shared.Interfaces;

namespace FiapGame.Domain.Usuario.Interfaces;

public interface IUsuarioRepository : IRepository<UsuarioEntity>
{
    Task<UsuarioEntity?> ObterPorEmailAsync(string email);
}
