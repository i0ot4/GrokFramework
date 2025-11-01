using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace Grok.Logging
{
    public static class HasLogLevelExtensions
    {
        public static TException WithLogLevel<TException>([NotNull] this TException exception, LogLevel logLevel)
            where TException : IHasLogLevel
        {
            exception.LogLevel = logLevel;

            return exception;
        }
    }
}
