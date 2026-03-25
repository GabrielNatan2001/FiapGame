using FiapGame.Shared.Base;
using FiapGame.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FiapGame.Infrastructure.Data.Repositories;

public class Repository<T> : IRepository<T> where T : BaseEntity
{
    protected readonly AppDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public Repository(AppDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<T?> ObterPorId(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<IEnumerable<T>> ObterTodos()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task Adicionar(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public void Atualizar(T entity)
    {
        _dbSet.Update(entity);
    }

    public void Remover(T entity)
    {
        _dbSet.Remove(entity);
    }

    public async Task<int> SalvarAlteracoes()
    {
        return await _context.SaveChangesAsync();
    }
}
