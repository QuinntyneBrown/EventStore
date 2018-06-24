using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using EventStore.Core.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace EventStore.API.Features.DomainEvents
{
    public class GetDomainEventsQuery
    {
        public class Request : IRequest<Response> { }

        public class Response
        {
            public IEnumerable<DomainEventApiModel> DomainEvents { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            public IAppDbContext _context { get; set; }
            
			public Handler(IAppDbContext context) => _context = context;

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
                => new Response()
                {
                    DomainEvents = await _context.DomainEvents.Select(x => DomainEventApiModel.FromDomainEvent(x)).ToListAsync()
                };
        }
    }
}
