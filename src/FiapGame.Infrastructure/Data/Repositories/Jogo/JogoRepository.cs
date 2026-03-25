using FiapGame.Domain.Jogo.Entities;
using FiapGame.Domain.Jogo.Interfaces;
using FiapGame.Infrastructure.Data.Repositories;

namespace FiapGame.Infrastructure.Data.Repositories.Jogo;

public class JogoRepository : Repository<JogoEntity>, IJogoRepository
{
    public JogoRepository(AppDbContext context) : base(context)
    {
    }
}
