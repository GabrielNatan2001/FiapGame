using FiapGame.Domain.Biblioteca.Entities;
using FiapGame.Domain.Jogo.Entities;
using FiapGame.Shared.Interfaces;

namespace FiapGame.Domain.Biblioteca.Interfaces;

public interface IBibliotecaRepository : IRepository<BibliotecaEntity>
{
    Task<BibliotecaEntity?> ObterPorUsuarioId(Guid usuarioId);
    Task<IReadOnlyCollection<JogoEntity>> ObterJogosDaBiblioteca(Guid usuarioId);
}
