using FiapGame.Domain.Usuario.Interfaces;
using FiapGame.Infrastructure.Data;
using FiapGame.Infrastructure.Data.Repositories.Usuario;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FiapGame.Infrastructure;

public static class DependencyInjectionInfraestructure
{
    public static IServiceCollection AddInfraestructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
        );

        services.AddScoped<IUsuarioRepository, UsuarioRepository>();

        return services;
    }
}
