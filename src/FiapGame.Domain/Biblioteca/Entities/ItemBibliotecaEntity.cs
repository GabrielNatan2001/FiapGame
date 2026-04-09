using System;
using FiapGame.Domain.Jogo.Entities;
using FiapGame.Shared.Base;

namespace FiapGame.Domain.Biblioteca.Entities;

public class ItemBibliotecaEntity : BaseEntity
{
    public Guid JogoId { get; private set; }
    public JogoEntity Jogo { get; private set; } = null!;
    public DateTime DataAquisicao { get; private set; }
    public Guid BibliotecaId { get; private set; }

    protected ItemBibliotecaEntity() { }

    private ItemBibliotecaEntity(Guid jogoId)
    {
        JogoId = jogoId;
        DataAquisicao = DateTime.UtcNow;
    }

    public static ItemBibliotecaEntity Criar(Guid jogoId) => new(jogoId);
}
