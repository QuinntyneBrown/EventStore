using MediatR;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using EventStore.Infrastructure.Data;
using EventStore.Infrastructure.Services;
using EventStore.Infrastructure.Requests;

namespace EventStore.Features.Tenants
{
    public class GetTenantsQuery
    {
        public class Request : BaseAuthenticatedRequest, IRequest<Response> { }

        public class Response
        {
            public ICollection<TenantApiModel> Tenants { get; set; } = new HashSet<TenantApiModel>();
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            public Handler(IEventStoreContext context, ICache cache)
            {
                _context = context;
                _cache = cache;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                return new Response()
                {
                    Tenants = await _context.Tenants.Select(x => TenantApiModel.FromTenant(x)).ToListAsync()
                };
            }

            private readonly IEventStoreContext _context;
            private readonly ICache _cache;
        }
    }
}
