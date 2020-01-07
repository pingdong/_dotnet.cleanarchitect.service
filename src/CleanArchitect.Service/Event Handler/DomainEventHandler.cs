using MediatR;
using PingDong.CleanArchitect.Core;
using PingDong.EventBus.Core;
using System;
using System.Threading.Tasks;

namespace PingDong.CleanArchitect.Service
{
    /// <summary>
    /// Provide the common functions for a DomainEvent handler
    /// </summary>
    public class DomainEventHandler
    {
        private readonly IEventBusPublisher _eventBus;
        private readonly IMediator _mediator;

        /// <summary>
        /// Initialize the object
        /// </summary>
        /// <param name="eventBus">The publisher to send a IntegrationEvent.</param>
        /// <param name="mediator">The mediator to send a Command</param>
        public DomainEventHandler(IEventBusPublisher eventBus, IMediator mediator)
        {
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        /// <summary>
        /// Publish an IntegrationEvent to external Event Bus
        /// </summary>
        /// <param name="tracker">The tracker</param>
        /// <param name="integrationEvent">The IntegrationEvent needs to be send</param>
        /// <returns></returns>
        protected async Task PublishAsync(IntegrationEvent integrationEvent, ITracker tracker = null)
        {
            if (integrationEvent == null)
                throw new ArgumentNullException(nameof(integrationEvent));

            if (tracker != null)
            {
                integrationEvent.CorrelationId = tracker.CorrelationId;
                integrationEvent.TenantId = tracker.TenantId;
            }

            await _eventBus.PublishAsync(integrationEvent).ConfigureAwait(false);
        }

        /// <summary>
        /// Dispatch a new Command
        /// </summary>
        /// <typeparam name="TResponse">The result type of the execution of the Command</typeparam>
        /// <param name="command">The Command is going to be sent</param>
        /// <param name="tracker">The tracker</param>
        /// <returns></returns>
        protected async Task<TResponse> DispatchAsync<TResponse>(Command<TResponse> command, ITracker tracker = null)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            if (tracker != null)
            {
                command.TenantId = tracker.TenantId;
                command.CorrelationId = tracker.CorrelationId;
            }

            return await _mediator.Send(command).ConfigureAwait(false);
        }
    }
}
