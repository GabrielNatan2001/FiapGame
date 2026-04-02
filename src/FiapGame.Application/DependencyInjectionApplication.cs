using FiapGame.Application.Jogo.Services;
using FiapGame.Application.Usuario.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FiapGame.Application;

public static class DependencyInjectionApplication
{
    public static IServiceCollection AddApplication(
    this IServiceCollection services,
    IConfiguration configuration)
    {
        services.AddScoped<CriarUsuarioService>();
        services.AddScoped<AutenticarUsuarioService>();
        services.AddScoped<CriarJogoService>();
        services.AddScoped<ListarJogosService>();
        services.AddScoped<ListarJogosAtivosService>();
        services.AddScoped<AtualizarJogoService>();
        services.AddScoped<AlterarStatusJogoService>();
        services.AddScoped<AdquirirJogoService>();
        services.AddScoped<ListarBibliotecaService>();

        return services;
    }
}
