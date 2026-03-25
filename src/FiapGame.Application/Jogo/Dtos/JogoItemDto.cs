namespace FiapGame.Application.Jogo.Dtos;

public sealed class JogoItemDto
{
    public Guid Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public decimal Preco { get; set; }
}
