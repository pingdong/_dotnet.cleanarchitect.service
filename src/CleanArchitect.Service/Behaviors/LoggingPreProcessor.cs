using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using MediatR.Pipeline;
using PingDong.CleanArchitect.Core;

namespace PingDong.CleanArchitect.Service
{
    public class LoggingPreProcessor<TRequest> : IRequestPreProcessor<TRequest>
    {
        private readonly ILogger<LoggingPreProcessor<TRequest>> _logger;

        public LoggingPreProcessor(ILogger<LoggingPreProcessor<TRequest>> logger)
        {
            _logger = logger;
        }

        public Task Process(TRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation(EventIds.Starting, $"Handled {typeof(TRequest).Name}");

            return Task.CompletedTask;
        }
    }
}
