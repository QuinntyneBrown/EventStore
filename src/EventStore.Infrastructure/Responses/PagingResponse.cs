using System.Collections.Generic;

namespace EventStore.Infrastructure.Responses
{
    public class PagingResponse<T>
    {
        public IEnumerable<T> Records { get; set; }
        public int TotalRecords { get; set; }

        public PagingResponse(IEnumerable<T> items, int totalRecords)
        {
            TotalRecords = totalRecords;
            Records = items;
        }
    }
}
