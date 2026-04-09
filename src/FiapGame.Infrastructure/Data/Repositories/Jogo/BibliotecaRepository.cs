using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FiapGame.Domain.Biblioteca.Entities;
using FiapGame.Domain.Biblioteca.Interfaces;
using FiapGame.Domain.Jogo.Entities;
using FiapGame.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FiapGame.Infrastructure.Data.Repositories.Jogo;

public class BibliotecaRepository : IBibliotecaRepository
{
    private readonly AppDbContext _context;

    public BibliotecaRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task Adicionar(BibliotecaEntity biblioteca)
    {
        await _context.Set<BibliotecaEntity>().AddAsync(biblioteca);
    }

    public void Atualizar(BibliotecaEntity biblioteca)
    {
        _context.Set<BibliotecaEntity>().Update(biblioteca);
    }

    public void Remover(BibliotecaEntity biblioteca)
    {
        _context.Set<BibliotecaEntity>().Remove(biblioteca);
    }

    public async Task<BibliotecaEntity?> ObterPorId(Guid id)
    {
        return await _context.Set<BibliotecaEntity>()
            .Include(x => x.Itens)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<BibliotecaEntity?> ObterPorUsuarioId(Guid usuarioId)
    {
        return await _context.Set<BibliotecaEntity>()
            .Include(x => x.Itens)
            .FirstOrDefaultAsync(x => x.UsuarioId == usuarioId);
    }

    public async Task<IEnumerable<BibliotecaEntity>> ObterTodos()
    {
        return await _context.Set<BibliotecaEntity>()
            .Include(x => x.Itens)
            .ToListAsync();
    }

    public async Task<IReadOnlyCollection<JogoEntity>> ObterJogosDaBiblioteca(Guid usuarioId)
    {
        var bibliotecaId = await _context.Set<BibliotecaEntity>()
            .Where(b => b.UsuarioId == usuarioId)
            .Select(b => b.Id)
            .FirstOrDefaultAsync();

        if (bibliotecaId == Guid.Empty)
            return new List<JogoEntity>();

        return await _context.Set<ItemBibliotecaEntity>()
            .Where(x => x.BibliotecaId == bibliotecaId)
            .Select(x => x.Jogo)
            .ToListAsync();
    }

    public async Task<int> SalvarAlteracoes()
    {
        return await _context.SaveChangesAsync();
    }
}
