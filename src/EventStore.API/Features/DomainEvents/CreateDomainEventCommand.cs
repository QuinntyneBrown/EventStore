using EventStore.Core.Interfaces;
using EventStore.Core.Models;
using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EventStore.API.Features.DomainEvents
{
    public class CreateDomainEventCommand
    {
        public class Validator: AbstractValidator<Request> {
            public Validator()
            {
                RuleFor(request => request.DomainEvent.DomainEventId).NotNull();
            }
        }

        public class Request : IRequest<Response> {
            public DomainEventApiModel DomainEvent { get; set; }
        }

        public class Response
        {			
            public int DomainEventId { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            public IAppDbContext _context { get; set; }
            
			public Handler(IAppDbContext context) => _context = context;

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var domainEvent = new DomainEvent();

                _context.DomainEvents.Add(domainEvent);
                
                await _context.SaveChangesAsync(cancellationToken);

                return new Response() { DomainEventId = domainEvent.DomainEventId };
            }
        }
    }
}
