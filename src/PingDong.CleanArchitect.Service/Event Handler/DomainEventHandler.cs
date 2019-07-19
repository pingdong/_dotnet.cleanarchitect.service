using System;
using System.Threading.Tasks;
using MediatR;
using PingDong.CleanArchitect.Core;
using PingDong.EventBus.Core;

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
        /// <param name="metadata">The metadata</param>
        /// <param name="integrationEvent">The IntegrationEvent needs to be send</param>
        /// <returns></returns>
        protected async Task PublishAsync(IntegrationEvent integrationEvent, IMetadata metadata = null)
        {
            if (integrationEvent == null)
                throw new ArgumentNullException(nameof(integrationEvent));

            if (metadata != null)
            {
                integrationEvent.CorrelationId = metadata.CorrelationId;
                integrationEvent.TenantId = metadata.TenantId;
            }
            
            await _eventBus.PublishAsync(integrationEvent).ConfigureAwait(false);
        }
        
        /// <summary>
        /// Dispatch a new Command
        /// </summary>
        /// <typeparam name="TResponse">The result type of the execution of the Command</typeparam>
        /// <param name="command">The Command is going to be sent</param>
        /// <param name="metadata">The metadata</param>
        /// <returns></returns>
        protected async Task<TResponse> DispatchAsync<TResponse>(Command<TResponse> command, IMetadata metadata = null)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            if (metadata != null)
            {
                command.TenantId = metadata.TenantId;
                command.CorrelationId = metadata.CorrelationId;
            }
            
            return await _mediator.Send(command).ConfigureAwait(false);
        }
    }
}
