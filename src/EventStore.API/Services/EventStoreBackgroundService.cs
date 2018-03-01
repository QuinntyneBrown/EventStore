using System.Threading;
using System.Threading.Tasks;
using EventStore.Infrastructure.Data;
using Microsoft.Extensions.Hosting;

namespace EventStore.API.Services
{
    public class CacheInvalidatorBackgroundService : BackgroundService
    {
        private readonly IEventStoreContext _context;
        public CacheInvalidatorBackgroundService(IEventStoreContext context)
        {
            _context = context;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.CompletedTask;
        }
    }
}
