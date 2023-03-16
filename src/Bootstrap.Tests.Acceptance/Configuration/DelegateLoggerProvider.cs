using System.Collections.Concurrent;
using BoDi;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging.Console;

namespace Bootstrap.Tests.Acceptance.Configuration;

public sealed class DelegateLoggerProvider : ILoggerProvider, ISupportExternalScope
{
    private readonly ConsoleFormatter _consoleFormatter;
    private readonly Action<string> _logCallback;
    private readonly ConcurrentDictionary<string, DelegateLogger> _loggers = new();
    private IExternalScopeProvider _scopeProvider = NullScopeProvider.Instance;

    public DelegateLoggerProvider(IEnumerable<ConsoleFormatter> consoleFormatter, Action<string> logCallback)
    {
        _logCallback = logCallback;
        var consoleFormatters = consoleFormatter as ConsoleFormatter[] ?? consoleFormatter.ToArray();
        _consoleFormatter = consoleFormatters.FirstOrDefault(f => f.Name == "simple") ?? consoleFormatters.First();
    }

    public ILogger CreateLogger(string categoryName) => _loggers.GetOrAdd(categoryName,
        key => new DelegateLogger(key, _consoleFormatter, _scopeProvider, WriteLine));

    public void Dispose() => _loggers.Clear();

    public void SetScopeProvider(IExternalScopeProvider scopeProvider)
    {
        _scopeProvider = scopeProvider;
        foreach (var o in _loggers.Values)
        {
            o.ScopeProvider = scopeProvider;
        }
    }

    private void WriteLine(string line)
    {
        try
        {
            _logCallback(line);
        }
        catch (ObjectContainerException)
        {
            // do not throw, it happens when specflow is disposed.
        }
    }

    private sealed class DelegateLogger : ILogger
    {
        private readonly string _categoryName;
        private readonly ConsoleFormatter _formatter;
        private readonly Action<string> _loggerDelegate;

        public DelegateLogger(string categoryName, ConsoleFormatter formatter, IExternalScopeProvider scopeProvider,
            Action<string> loggerDelegate)
        {
            _categoryName = categoryName;
            _formatter = formatter;
            ScopeProvider = scopeProvider;
            _loggerDelegate = loggerDelegate;
        }

        public IExternalScopeProvider? ScopeProvider { get; set; }

        public IDisposable BeginScope<TState>(TState state) where TState : notnull =>
            ScopeProvider?.Push(state) ?? NullScope.Instance;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
            var stringWriter = new StringWriter();
            LogEntry<TState> logEntry = new(logLevel, _categoryName, eventId, state, exception, formatter);
            _formatter.Write(in logEntry, ScopeProvider, stringWriter);
            _loggerDelegate(stringWriter.ToString());
        }
    }

    private sealed class NullScope : IDisposable
    {
        private NullScope() { }
        public static IDisposable Instance { get; } = new NullScope();

        public void Dispose()
        {
            // Nothing to do here
        }
    }

    private sealed class NullScopeProvider : IExternalScopeProvider
    {
        private NullScopeProvider() { }
        public static IExternalScopeProvider Instance { get; } = new NullScopeProvider();

        public void ForEachScope<TState>(Action<object, TState> callback, TState state)
        {
            // Nothing to do here
        }

        public IDisposable Push(object? state) => NullScope.Instance;
    }
}
