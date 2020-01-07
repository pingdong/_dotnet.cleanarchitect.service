using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using PingDong.CleanArchitect.Core;
using System.Threading;
using System.Threading.Tasks;

namespace PingDong.CleanArchitect.Service
{
    public class LoggingPostProcessor<TRequest, TResponse> : IRequestPostProcessor<TRequest, TResponse>
    {
        private readonly ILogger<LoggingPostProcessor<TRequest, TResponse>> _logger;

        public LoggingPostProcessor(ILogger<LoggingPostProcessor<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public Task Process(TRequest request, TResponse response, CancellationToken cancellationToken)
        {
            _logger.LogInformation(EventIds.Success, $"Handled {typeof(TResponse).Name}");

            return Task.CompletedTask;
        }
    }
}
