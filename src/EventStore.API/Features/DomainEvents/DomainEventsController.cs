using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EventStore.API.Features.DomainEvents
{
    [Authorize]
    [ApiController]
    [Route("api/domainEvents")]
    public class DomainEventsController
    {
        private readonly IMediator _mediator;

        public DomainEventsController(IMediator mediator) => _mediator = mediator;

        [HttpPost]
        public async Task<ActionResult<CreateDomainEventCommand.Response>> Post(CreateDomainEventCommand.Request request)
            => await _mediator.Send(request);
        
        [HttpGet]
        public async Task<ActionResult<GetDomainEventsQuery.Response>> Get()
            => await _mediator.Send(new GetDomainEventsQuery.Request());
    }
}
