namespace FiapGame.Shared.Base;

public abstract class BaseEntity
{
    public Guid Id { get; protected set; }
    public DateTime DtCadastro { get; protected set; } = DateTime.UtcNow;
    public DateTime? DtAtualizacao { get; protected set; }

    public void AtualizarDataAtualizacao()
    {
        DtAtualizacao = DateTime.UtcNow;
    }
}
