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

        return services;
    }
}
