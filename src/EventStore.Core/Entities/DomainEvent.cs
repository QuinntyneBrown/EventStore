using System;

namespace EventStore.Core.Entities
{
    public class DomainEvent
    {
        public Guid DomainEventId { get; set; }
        public dynamic Data { get; set; }
    }
}
