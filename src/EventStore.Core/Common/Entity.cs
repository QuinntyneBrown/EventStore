using MediatR;
using System;
using System.Collections.Generic;

namespace EventStore.Core.Common
{
    public class Entity
    {
        public Entity() => _domainEvents = new List<INotification>();        
        private List<INotification> _domainEvents;
        public IReadOnlyCollection<INotification> DomainEvents => _domainEvents.AsReadOnly();
        public void RaiseDomainEvent(INotification eventItem) => _domainEvents.Add(eventItem);
        public void ClearEvents() => _domainEvents.Clear();
    }
}
