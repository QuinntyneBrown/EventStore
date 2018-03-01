using System;
using EventStore.Core.Interfaces;

namespace EventStore.Core.Entities
{
    public class BaseModel: ILoggable
    {
        public Tenant Tenant { get; set; }
        public bool IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime LastModifiedOn { get; set; }
    }
}
