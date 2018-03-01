using EventStore.Core.Entities;
using System;

namespace EventStore.Features.Tenants
{
    public class TenantApiModel
    {        
        public Guid TenantId { get; set; }
        public string Name { get; set; }

        public static TenantApiModel FromTenant(Tenant tenant)
        {
            var model = new TenantApiModel();
            model.TenantId = tenant.TenantId;
            model.Name = tenant.Name;
            return model;
        }
    }
}
