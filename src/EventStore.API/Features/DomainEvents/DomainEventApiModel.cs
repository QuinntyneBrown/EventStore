using EventStore.Core.Models;
using System;

namespace EventStore.API.Features.DomainEvents
{
    public class DomainEventApiModel
    {        
        public int DomainEventId { get; set; }
        
        public static DomainEventApiModel FromDomainEvent(DomainEvent domainEvent)
            => new DomainEventApiModel
            {
                DomainEventId = domainEvent.DomainEventId
            };
    }
}
