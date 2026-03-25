using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Options;

namespace FiapGame.API.Logging;

public sealed class CorrelationConsoleFormatter : ConsoleFormatter, IDisposable
{
    public const string FormatterName = "correlation";

    private readonly IDisposable? _optionsReloadToken;
    private CorrelationConsoleFormatterOptions _options;

    public CorrelationConsoleFormatter(IOptionsMonitor<CorrelationConsoleFormatterOptions> options)
        : base(FormatterName)
    {
        _options = options.CurrentValue;
        _optionsReloadToken = options.OnChange(updated => _options = updated);
    }

    public override void Write<TState>(in LogEntry<TState> logEntry, IExternalScopeProvider? scopeProvider, TextWriter textWriter)
    {
        var message = logEntry.Formatter(logEntry.State, logEntry.Exception);

        if (string.IsNullOrWhiteSpace(message) && logEntry.Exception is null)
        {
            return;
        }

        var level = GetLevelCode(logEntry.LogLevel);
        var correlationId = TryGetCorrelationId(scopeProvider) ?? "-";
        var source = GetShortCategoryName(logEntry.Category);

        textWriter.Write($"[{level}] [CorrelationId: {correlationId}] {source}");

        if (!string.IsNullOrWhiteSpace(message))
        {
            textWriter.Write($" - {message}");
        }

        if (logEntry.Exception is not null)
        {
            textWriter.Write($" | Exception: {logEntry.Exception}");
        }

        textWriter.WriteLine();
    }

    public void Dispose()
    {
        _optionsReloadToken?.Dispose();
    }

    private static string GetShortCategoryName(string category)
    {
        if (string.IsNullOrWhiteSpace(category))
        {
            return "App";
        }

        var lastDot = category.LastIndexOf('.');
        return lastDot >= 0 && lastDot < category.Length - 1
            ? category[(lastDot + 1)..]
            : category;
    }

    private static string? TryGetCorrelationId(IExternalScopeProvider? scopeProvider)
    {
        if (scopeProvider is null)
        {
            return null;
        }

        string? correlationId = null;

        scopeProvider.ForEachScope((scope, _) =>
        {
            if (scope is IEnumerable<KeyValuePair<string, object>> values)
            {
                foreach (var value in values)
                {
                    if (!string.Equals(value.Key, "CorrelationId", StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    correlationId = value.Value?.ToString();
                    return;
                }
            }
        }, state: (object?)null);

        return correlationId;
    }

    private static string GetLevelCode(LogLevel logLevel) =>
        logLevel switch
        {
            LogLevel.Trace => "TRC",
            LogLevel.Debug => "DBG",
            LogLevel.Information => "INF",
            LogLevel.Warning => "WRN",
            LogLevel.Error => "ERR",
            LogLevel.Critical => "CRT",
            _ => "UNK"
        };
}
