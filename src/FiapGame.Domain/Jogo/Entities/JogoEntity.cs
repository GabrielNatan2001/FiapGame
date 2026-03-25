using FiapGame.Shared.Base;
using FiapGame.Shared.Exceptions;

namespace FiapGame.Domain.Jogo.Entities;

public class JogoEntity : BaseEntity
{
    public string Titulo { get; private set; }
    public string Descricao { get; private set; }
    public decimal Preco { get; private set; }

    protected JogoEntity() { }

    private JogoEntity(string titulo, string descricao, decimal preco)
    {
        Titulo = titulo;
        Descricao = descricao;
        Preco = preco;
    }

    public static JogoEntity Criar(string titulo, string descricao, decimal preco)
    {
        if (string.IsNullOrWhiteSpace(titulo))
            throw new DomainException("Titulo do jogo é obrigatório.");

        if (preco < 0)
            throw new DomainException("Preço do jogo não pode ser negativo.");

        return new JogoEntity(titulo.Trim(), descricao?.Trim() ?? string.Empty, preco);
    }
}
