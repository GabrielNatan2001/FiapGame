using FiapGame.Domain.Jogo.Entities;
using FiapGame.Shared.Interfaces;

namespace FiapGame.Domain.Jogo.Interfaces;

public interface IUsuarioJogoRepository : IRepository<UsuarioJogoEntity>
{
    Task<bool> UsuarioPossuiJogo(Guid usuarioId, Guid jogoId);
    Task<IReadOnlyCollection<JogoEntity>> ObterBiblioteca(Guid usuarioId);
}
