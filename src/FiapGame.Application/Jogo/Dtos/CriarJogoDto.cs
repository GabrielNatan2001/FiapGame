namespace FiapGame.Application.Jogo.Dtos;

public static class CriarJogoDto
{
    public sealed class Request
    {
        public string Titulo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public decimal Preco { get; set; }
    }
}
