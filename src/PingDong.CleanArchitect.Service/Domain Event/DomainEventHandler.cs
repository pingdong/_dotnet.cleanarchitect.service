using System;
using System.Threading.Tasks;
using PingDong.CleanArchitect.Core;
using PingDong.EventBus.Core;

namespace PingDong.CleanArchitect.Service
{
    public class EventBusDomainEventHandler
    {
        private readonly IEventBusPublisher _eventBus;

        public EventBusDomainEventHandler(IEventBusPublisher eventBus)
        {
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        }

        protected async Task PublishAsync(DomainEvent domainEvent, IntegrationEvent integrationEvent)
        {
            integrationEvent.CorrelationId = domainEvent.CorrelationId;
            integrationEvent.TenantId = domainEvent.TenantId;

            await _eventBus.PublishAsync(integrationEvent);
        }
    }
}
