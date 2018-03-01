using EventStore.Infrastructure.Data;
using EventStore.Infrastructure.Services;
using MediatR;
using System;
using System.Threading.Tasks;
using System.Threading;

namespace EventStore.Features.Tenants
{
    public class VerifyTenantCommand
    {
        public class Request : IRequest
        {
            public Guid TenantId { get; set; }
        }

        public class Handler : IRequestHandler<Request>
        {
            public Handler(IEventStoreContext context, ICache cache)
            {
                _context = context;
                _cache = cache;
            }

            public Task Handle(Request request, CancellationToken cancellationToken)
            {
                if (request.TenantId != new Guid("bad9a182-ede0-418d-9588-2d89cfd555bd"))
                    throw new Exception("Invalid Request");

                return Task.CompletedTask;
            }

            private readonly IEventStoreContext _context;
            private readonly ICache _cache;
        }
    }
}
