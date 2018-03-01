using System;

namespace EventStore.Core.Entities
{
    public class Tenant
    {
        public Guid TenantId { get; set; }           
		public string Name { get; set; }        
    }
}
