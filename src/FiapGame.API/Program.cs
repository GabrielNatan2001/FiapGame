using FiapGame.API.Logging;
using FiapGame.API.Middlewares;
using FiapGame.Application;
using FiapGame.Infrastructure;

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
builder.Services.AddSwaggerGen();

builder.Services.AddInfraestructure(builder.Configuration);
builder.Services.AddApplication(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<LoggingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
