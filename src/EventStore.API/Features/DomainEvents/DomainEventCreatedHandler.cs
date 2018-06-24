using EventStore.Core;
using EventStore.Core.DomainEvents;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EventStore.API.Features.DomainEvents
{
    public class DomainEventCreatedHandler : INotificationHandler<DomainEventCreated>
    {
        private readonly IHubContext<DomainEventsHub> _hubContext;

        public DomainEventCreatedHandler(IHubContext<DomainEventsHub> hubContext)
            => _hubContext = hubContext;

        public Task Handle(DomainEventCreated notification, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
