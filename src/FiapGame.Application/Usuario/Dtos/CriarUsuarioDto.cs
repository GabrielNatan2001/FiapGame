namespace FiapGame.Application.Usuario.Dtos;

public class CriarUsuarioDto
{
    public class Request
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
    }
}
