using FiapGame.Domain.Jogo.Entities;
using FiapGame.Domain.Jogo.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FiapGame.Infrastructure.Data.Repositories.Jogo;

public class UsuarioJogoRepository : Repository<UsuarioJogoEntity>, IUsuarioJogoRepository
{
    public UsuarioJogoRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<bool> UsuarioPossuiJogo(Guid usuarioId, Guid jogoId)
    {
        return await _dbSet.AnyAsync(x => x.UsuarioId == usuarioId && x.JogoId == jogoId);
    }

    public async Task<IReadOnlyCollection<JogoEntity>> ObterBiblioteca(Guid usuarioId)
    {
        return await _dbSet
            .AsNoTracking()
            .Where(x => x.UsuarioId == usuarioId)
            .Include(x => x.Jogo)
            .Select(x => x.Jogo)
            .ToListAsync();
    }
}
