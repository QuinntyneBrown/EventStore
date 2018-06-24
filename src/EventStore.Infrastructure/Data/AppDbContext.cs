using EventStore.Core.Models;
using EventStore.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Generic;
using EventStore.Core.Common;

namespace EventStore.Infrastructure.Data
{    
    public class AppDbContext : DbContext, IAppDbContext
    {
        private readonly IMediator _mediator;
        public AppDbContext(DbContextOptions options, IMediator mediator = default(IMediator))
            :base(options) {
            _mediator = mediator;
        }

        public static readonly LoggerFactory ConsoleLoggerFactory
            = new LoggerFactory(new[] {
                new ConsoleLoggerProvider((category, level)
                    => category == DbLoggerCategory.Database.Command.Name 
                && level == LogLevel.Information, true) });
        public DbSet<DomainEvent> DomainEvents { get; set; }
        
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            var result = default(int);
            
            var domainEventEntities = ChangeTracker.Entries<Entity>()
                .Select(entityEntry => entityEntry.Entity)
                .Where(entity => entity.DomainEvents.Any())
                .ToArray();
            
            result = await base.SaveChangesAsync(cancellationToken);

            foreach (var entity in domainEventEntities)
            {
                var events = entity.DomainEvents.ToArray();

                entity.ClearEvents();

                foreach (var domainEvent in events)
                {
                    await _mediator.Publish(domainEvent, cancellationToken);
                }
            }

            return result;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }       
    }
}