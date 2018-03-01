using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace EventStore.API.Services
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class EventStoreHub: Hub
    {
    }
}
