using MediatR;
using System.Threading.Tasks;
using System.Threading;
using EventStore.Infrastructure.Data;
using EventStore.Infrastructure.Services;
using EventStore.Infrastructure.Requests;

namespace EventStore.Features.Users
{
    public class GetUserByIdQuery
    {
        public class Request : BaseAuthenticatedRequest, IRequest<Response> { 
            public int Id { get; set; }            
        }

        public class Response
        {
            public UserApiModel User { get; set; }
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
                    User = UserApiModel.FromUser(await _context.Users.FindAsync(request.Id))
                };
            }

            private readonly IEventStoreContext _context;
            private readonly ICache _cache;
        }
    }
}
