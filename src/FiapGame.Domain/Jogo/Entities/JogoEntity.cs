using FiapGame.Domain.Common.Enums;
using FiapGame.Shared.Base;
using FiapGame.Shared.Exceptions;

namespace FiapGame.Domain.Jogo.Entities;

public class JogoEntity : BaseEntity
{
    public string Nome { get; private set; }
    public string Descricao { get; private set; }
    public decimal Preco { get; private set; }
    public string Categoria { get; private set; }
    public EStatus Status { get; private set; }

    protected JogoEntity() { }

    private JogoEntity(string nome, string descricao, decimal preco, string categoria, EStatus status)
    {
        Nome = nome;
        Descricao = descricao;
        Preco = preco;
        Categoria = categoria;
        Status = status;
    }

    public static JogoEntity Criar(string nome, string descricao, decimal preco, string categoria)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new DomainException("Nome do jogo é obrigatório.");

        if (preco < 0)
            throw new DomainException("Preço do jogo não pode ser negativo.");

        if (string.IsNullOrWhiteSpace(categoria))
            throw new DomainException("Categoria do jogo é obrigatória.");

        return new JogoEntity(nome.Trim(), descricao?.Trim() ?? string.Empty, preco, categoria.Trim(), EStatus.Ativo);
    }

    public void Atualizar(string nome, string descricao, decimal preco, string categoria)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new DomainException("Nome do jogo é obrigatório.");

        if (preco < 0)
            throw new DomainException("Preço do jogo não pode ser negativo.");

        if (string.IsNullOrWhiteSpace(categoria))
            throw new DomainException("Categoria do jogo é obrigatória.");

        Nome = nome.Trim();
        Descricao = descricao?.Trim() ?? string.Empty;
        Preco = preco;
        Categoria = categoria.Trim();
    }

    public void AlterarStatus()
    {
        Status = Status == EStatus.Ativo ? EStatus.Inativo : EStatus.Ativo;
    }
}
