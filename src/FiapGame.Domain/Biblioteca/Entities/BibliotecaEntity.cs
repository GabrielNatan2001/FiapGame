using System;
using System.Collections.Generic;
using System.Linq;
using FiapGame.Shared.Base;
using FiapGame.Shared.Exceptions;

namespace FiapGame.Domain.Biblioteca.Entities;

public class BibliotecaEntity : BaseEntity
{
    public Guid UsuarioId { get; private set; }
    private readonly List<ItemBibliotecaEntity> _itens = new();
    public IReadOnlyCollection<ItemBibliotecaEntity> Itens => _itens.AsReadOnly();

    protected BibliotecaEntity() { }

    private BibliotecaEntity(Guid usuarioId)
    {
        UsuarioId = usuarioId;
    }

    public static BibliotecaEntity Criar(Guid usuarioId) => new(usuarioId);

    public void AdicionarJogo(Guid jogoId)
    {
        if (PossuiJogo(jogoId))
            throw new DomainException("Este jogo já está na sua biblioteca.");

        _itens.Add(ItemBibliotecaEntity.Criar(jogoId));
        AtualizarDataAtualizacao();
    }

    public bool PossuiJogo(Guid jogoId)
    {
        return _itens.Any(x => x.JogoId == jogoId);
    }
}
