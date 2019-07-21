using System;
using System.Threading.Tasks;
using MediatR;
using PingDong.CleanArchitect.Core;

namespace PingDong.CleanArchitect.Service
{
    /// <summary>
    /// Provide the common functions for an IntegrationEvent handler
    /// </summary>
    public class IntegrationEventHandler
    {
        private readonly IMediator _mediator;
        
        /// <summary>
        /// Initialize the object
        /// </summary>
        /// <param name="mediator">The mediator to send a Command</param>
        public IntegrationEventHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Dispatch a new Command
        /// </summary>
        /// <typeparam name="TResponse">The result type of the execution of the Command</typeparam>
        /// <param name="command">The Command is going to be sent</param>
        /// <param name="event">The IntegrationEvent</param>
        protected async Task<TResponse> DispatchAsync<TResponse>(Command<TResponse> command, IntegrationEvent @event = null)
        {
            IRequest<TResponse> cmd;

            if (@event != null)
            {
                if (string.IsNullOrWhiteSpace(@event.RequestId))
                    return default;

                if (!Guid.TryParse(@event.RequestId, out var requestId))
                    return default;

                command.CorrelationId = @event.CorrelationId;
                command.TenantId = @event.TenantId;

                cmd = new IdentifiedCommand<Guid, TResponse, Command<TResponse>>(requestId, command);
            }
            else
                cmd = command;

            return await _mediator.Send(cmd).ConfigureAwait(false);
        }
    }
}