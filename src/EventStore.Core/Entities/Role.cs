namespace EventStore.Core.Entities
{
    public class Role: BaseModel
    {
        public int RoleId { get; set; }           
		public string Name { get; set; }        
    }
}
