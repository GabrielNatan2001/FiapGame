using FiapGame.Shared.Base;

namespace FiapGame.Shared.Interfaces;

public interface IRepository<T> where T : BaseEntity
{
    Task<T?> ObterPorId(Guid id);
    Task<IEnumerable<T>> ObterTodos();

    Task Adicionar(T entity);
    void Atualizar(T entity);
    void Remover(T entity);

    Task<int> SalvarAlteracoes();
}
