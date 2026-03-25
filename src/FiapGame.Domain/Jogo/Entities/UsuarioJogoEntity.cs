using FiapGame.Domain.Usuario.Entities;
using FiapGame.Shared.Base;

namespace FiapGame.Domain.Jogo.Entities;

public class UsuarioJogoEntity : BaseEntity
{
    public Guid UsuarioId { get; private set; }
    public UsuarioEntity Usuario { get; private set; } = null!;
    public Guid JogoId { get; private set; }
    public JogoEntity Jogo { get; private set; } = null!;

    protected UsuarioJogoEntity() { }

    private UsuarioJogoEntity(Guid usuarioId, Guid jogoId)
    {
        UsuarioId = usuarioId;
        JogoId = jogoId;
    }

    public static UsuarioJogoEntity Criar(Guid usuarioId, Guid jogoId) => new(usuarioId, jogoId);
}
