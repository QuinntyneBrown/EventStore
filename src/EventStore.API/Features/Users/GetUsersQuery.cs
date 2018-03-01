using MediatR;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using EventStore.Infrastructure.Data;
using EventStore.Infrastructure.Services;
using EventStore.Infrastructure.Requests;

namespace EventStore.Features.Users
{
    public class GetUsersQuery
    {
        public class Request : BaseAuthenticatedRequest, IRequest<Response> { }

        public class Response
        {
            public ICollection<UserApiModel> Users { get; set; } = new HashSet<UserApiModel>();
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
                    Users = await _context.Users.Select(x => UserApiModel.FromUser(x)).ToListAsync()
                };
            }

            private readonly IEventStoreContext _context;
            private readonly ICache _cache;
        }
    }
}
