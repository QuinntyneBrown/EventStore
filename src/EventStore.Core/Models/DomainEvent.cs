using EventStore.Core.Common;

namespace EventStore.Core.Models
{
    public class DomainEvent: Entity
    {
        public int DomainEventId { get; set; }
        public string Type { get; set; }
        public string EntityType { get; set; }
        public string EntityId { get; set; }
        public string AggregateId { get; set; }        
        public string Data { get; set; }
    }
}
