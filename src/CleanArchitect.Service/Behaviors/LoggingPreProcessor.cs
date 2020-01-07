using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using PingDong.CleanArchitect.Core;
using System.Threading;
using System.Threading.Tasks;

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
