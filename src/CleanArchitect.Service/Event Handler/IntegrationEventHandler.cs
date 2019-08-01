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
        /// <typeparam name="TCommand"></typeparam>
        /// <param name="command">The Command is going to be sent</param>
        /// <param name="event">The IntegrationEvent</param>
        protected async Task<bool> DispatchAsync<TCommand>(TCommand command, IntegrationEvent @event)
            where TCommand: Command<bool>
        {
            if (@event == null)
                throw new ArgumentNullException(nameof(@event));

            command.CorrelationId = @event.CorrelationId;
            command.TenantId = @event.TenantId;
            if (!Guid.TryParse(@event.RequestId, out Guid requestId))
                throw new ArgumentNullException();

            var cmd = new IdentifiedCommand<Guid, bool, TCommand>(requestId, command);

            return await _mediator.Send(cmd).ConfigureAwait(false);
        }
    }
}