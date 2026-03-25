using FiapGame.Domain.Usuario.Entities;
using FiapGame.Domain.Usuario.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FiapGame.Infrastructure.Data.Repositories.Usuario;

public class UsuarioRepository : Repository<UsuarioEntity>, IUsuarioRepository
{
    public UsuarioRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<UsuarioEntity?> ObterPorEmailAsync(string email)
    {
        return await _dbSet.FirstOrDefaultAsync(x => x.Email.Value == email);
    }
}
