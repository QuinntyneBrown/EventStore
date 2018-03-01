using System;

namespace EventStore.Infrastructure.Requests
{
    public class BaseRequest
    {
        public Guid TenantId { get; set; }
    }
}
