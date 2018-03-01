using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using EventStore.Infrastructure.Data;
using EventStore.Infrastructure.Services;
using EventStore.Infrastructure.Requests;
using EventStore.Core.Entities;

namespace EventStore.Features.Tenants
{
    public class RemoveTenantCommand
    {
        public class Request : BaseAuthenticatedRequest, IRequest { 
            public int Id { get; set; }            
        }

        public class Handler : IRequestHandler<Request>
        {
            public Handler(IEventStoreContext context, ICache cache)
            {
                _context = context;
                _cache = cache;
            }

            public async Task Handle(Request request, CancellationToken cancellationToken)
            {
                _context.Tenants.Remove(await _context.Tenants.FindAsync(request.Id));
                await _context.SaveChangesAsync(cancellationToken, request.Username);
            }

            private readonly IEventStoreContext _context;
            private readonly ICache _cache;
        }
    }
}
