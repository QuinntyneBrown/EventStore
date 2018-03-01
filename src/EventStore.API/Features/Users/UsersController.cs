using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace EventStore.Features.Users
{
    [Authorize]
    [Route("api/users")]
    public class UsersController : Controller
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Route("add")]
        [HttpPost]
        [ProducesResponseType(typeof(AddOrUpdateUserCommand.Response),(int)HttpStatusCode.OK)]
        public async Task<IActionResult> Add([FromBody]AddOrUpdateUserCommand.Request request)
            => Ok(await _mediator.Send(request));
        
        [Route("update")]
        [HttpPut]
        [ProducesResponseType(typeof(AddOrUpdateUserCommand.Response),(int)HttpStatusCode.OK)]
        public async Task<IActionResult> Update([FromBody]AddOrUpdateUserCommand.Request request)
            => Ok(await _mediator.Send(request));
        
        [Route("get")]
        [HttpGet]
        [ProducesResponseType(typeof(GetUsersQuery.Response),(int)HttpStatusCode.OK)] 
        public async Task<IActionResult> Get()
            => Ok(await _mediator.Send(new GetUsersQuery.Request()));

        [Route("getById")]
        [HttpGet]
        [ProducesResponseType(typeof(GetUserByIdQuery.Response),(int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetById([FromQuery]GetUserByIdQuery.Request request)
            => Ok(await _mediator.Send(request));

        [Route("remove")]
        [HttpDelete]
        [ProducesResponseType(typeof(void),(int)HttpStatusCode.OK)]
        public async Task<IActionResult> Remove([FromQuery]RemoveUserCommand.Request request) {
            await _mediator.Send(request);
            return Ok();
        }
    }
}
