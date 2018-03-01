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
    public class GetTenantByIdQuery
    {
        public class Request : BaseAuthenticatedRequest, IRequest<Response> { 
            public int Id { get; set; }            
        }

        public class Response
        {
            public TenantApiModel Tenant { get; set; }
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
                    Tenant = TenantApiModel.FromTenant(await _context.Tenants.FindAsync(request.Id))
                };
            }

            private readonly IEventStoreContext _context;
            private readonly ICache _cache;
        }
    }
}
