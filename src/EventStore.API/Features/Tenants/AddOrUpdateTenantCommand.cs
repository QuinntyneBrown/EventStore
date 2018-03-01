using MediatR;
using System.Threading.Tasks;
using System.Threading;
using EventStore.Infrastructure.Data;
using EventStore.Infrastructure.Services;
using EventStore.Infrastructure.Requests;
using EventStore.Core.Entities;
using System;

namespace EventStore.Features.Tenants
{
    public class AddOrUpdateTenantCommand
    {
        public class Request : BaseAuthenticatedRequest, IRequest<Response>
        {            
            public TenantApiModel Tenant { get; set; }
        }

        public class Response
        {            
            public Guid TenantId { get; set; }
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
                var tenant = await _context.Tenants.FindAsync(request.Tenant.TenantId);
                
                if (tenant == null)
                    _context.Tenants.Add(tenant = new Tenant());

                tenant.Name = request.Tenant.Name;
                
                await _context.SaveChangesAsync(cancellationToken, request.Username);

                return new Response() { TenantId = tenant.TenantId };
            }

            private readonly IEventStoreContext _context;
            private readonly ICache _cache;
        }

    }

}
