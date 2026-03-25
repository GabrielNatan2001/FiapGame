namespace FiapGame.Application.Usuario.Dtos;

public static class AutenticarUsuarioDto
{
    public sealed class Request
    {
        public string Email { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
    }

    public sealed class Response
    {
        public string AccessToken { get; set; } = string.Empty;
        public string TokenType { get; set; } = "Bearer";
        public DateTime ExpiraEmUtc { get; set; }
    }
}
