using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using EventStore.Infrastructure.Data;
using EventStore.Infrastructure.Services;
using EventStore.Infrastructure.Requests;
using EventStore.Core.Entities;

namespace EventStore.Features.Roles
{
    public class AddOrUpdateRoleCommand
    {
        public class Request : BaseAuthenticatedRequest, IRequest<Response>
        {            
            public RoleApiModel Role { get; set; }
        }

        public class Response
        {            
            public int RoleId { get; set; }
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
                var role = await _context.Roles.FindAsync(request.Role.RoleId);
                
                if (role == null)
                    _context.Roles.Add(role = new Role());

                role.Name = request.Role.Name;
                
                await _context.SaveChangesAsync(cancellationToken, request.Username);

                return new Response() { RoleId = role.RoleId };
            }

            private readonly IEventStoreContext _context;
            private readonly ICache _cache;
        }

    }

}
