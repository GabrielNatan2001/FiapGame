using FiapGame.API.Logging;
using FiapGame.API.Middlewares;
using FiapGame.API.Services;
using FiapGame.Application;
using FiapGame.Domain.Usuario.Entities;
using FiapGame.Infrastructure.Data;
using FiapGame.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Logging
builder.Logging.ClearProviders();

builder.Logging.AddConsoleFormatter<CorrelationConsoleFormatter, CorrelationConsoleFormatterOptions>();
builder.Logging.AddConsole(options =>
{
    options.FormatterName = CorrelationConsoleFormatter.FormatterName;
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.CustomSchemaIds(type => type.FullName?.Replace("+", "."));
});

builder.Services.AddInfraestructure(builder.Configuration);
builder.Services.AddApplication(builder.Configuration);
builder.Services.AddScoped<JwtTokenProvider>();
builder.Services.AddScoped<FiapGame.Application.Abstractions.Security.ITokenProvider, JwtTokenProvider>();

var jwtKey = builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key não configurado.");
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "FiapGame";
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? "FiapGame.Client";

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

var app = builder.Build();
await SeedAdminAsync(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<LoggingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

static async Task SeedAdminAsync(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    await dbContext.Database.MigrateAsync();

    const string adminEmail = "admin@admin.com";
    var adminExistente = await dbContext.Usuarios.FirstOrDefaultAsync(u => u.Email.Value == adminEmail);
    if (adminExistente is not null)
    {
        return;
    }

    var admin = UsuarioEntity.CriarAdmin("admin", adminEmail, "Teste@123");
    await dbContext.Usuarios.AddAsync(admin);
    await dbContext.SaveChangesAsync();
}
