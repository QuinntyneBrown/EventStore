using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace EventStore.Features.Tenants
{
    [Authorize]
    [Route("api/tenants")]
    public class TenantsController : Controller
    {
        private readonly IMediator _mediator;

        public TenantsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Route("add")]
        [HttpPost]
        [ProducesResponseType(typeof(AddOrUpdateTenantCommand.Response),(int)HttpStatusCode.OK)]
        public async Task<IActionResult> Add([FromBody]AddOrUpdateTenantCommand.Request request)
            => Ok(await _mediator.Send(request));
        
        [Route("update")]
        [HttpPut]
        [ProducesResponseType(typeof(AddOrUpdateTenantCommand.Response),(int)HttpStatusCode.OK)]
        public async Task<IActionResult> Update([FromBody]AddOrUpdateTenantCommand.Request request)
            => Ok(await _mediator.Send(request));
        
        [Route("get")]
        [HttpGet]
        [ProducesResponseType(typeof(GetTenantsQuery.Response),(int)HttpStatusCode.OK)] 
        public async Task<IActionResult> Get()
            => Ok(await _mediator.Send(new GetTenantsQuery.Request()));

        [Route("getById")]
        [HttpGet]
        [ProducesResponseType(typeof(GetTenantByIdQuery.Response),(int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetById([FromQuery]GetTenantByIdQuery.Request request)
            => Ok(await _mediator.Send(request));

        [Route("remove")]
        [HttpDelete]
        [ProducesResponseType(typeof(void),(int)HttpStatusCode.OK)]
        public async Task<IActionResult> Remove([FromQuery]RemoveTenantCommand.Request request) {
            await _mediator.Send(request);
            return Ok();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("verify")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Verify([FromBody]VerifyTenantCommand.Request request)
        {
            await _mediator.Send(request);
            return Ok();
        }
    }
}
