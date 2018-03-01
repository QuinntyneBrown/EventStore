using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace EventStore.Features.Roles
{
    [Authorize]
    [Route("api/roles")]
    public class RolesController : Controller
    {
        private readonly IMediator _mediator;

        public RolesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Route("add")]
        [HttpPost]
        [ProducesResponseType(typeof(AddOrUpdateRoleCommand.Response),(int)HttpStatusCode.OK)]
        public async Task<IActionResult> Add([FromBody]AddOrUpdateRoleCommand.Request request)
            => Ok(await _mediator.Send(request));
        
        [Route("update")]
        [HttpPut]
        [ProducesResponseType(typeof(AddOrUpdateRoleCommand.Response),(int)HttpStatusCode.OK)]
        public async Task<IActionResult> Update([FromBody]AddOrUpdateRoleCommand.Request request)
            => Ok(await _mediator.Send(request));
        
        [Route("get")]
        [HttpGet]
        [ProducesResponseType(typeof(GetRolesQuery.Response),(int)HttpStatusCode.OK)] 
        public async Task<IActionResult> Get()
            => Ok(await _mediator.Send(new GetRolesQuery.Request()));

        [Route("getById")]
        [HttpGet]
        [ProducesResponseType(typeof(GetRoleByIdQuery.Response),(int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetById([FromQuery]GetRoleByIdQuery.Request request)
            => Ok(await _mediator.Send(request));

        [Route("remove")]
        [HttpDelete]
        [ProducesResponseType(typeof(void),(int)HttpStatusCode.OK)]
        public async Task<IActionResult> Remove([FromQuery]RemoveRoleCommand.Request request) {
            await _mediator.Send(request);
            return Ok();
        }
    }
}
