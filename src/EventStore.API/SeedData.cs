using EventStore.Infrastructure.Data;

namespace EventStore.API
{
    public class SeedData
    {
        public static void Seed(AppDbContext context)
        {
            UserConfiguration.Seed(context);
            TagConfiguration.Seed(context);

            context.SaveChanges();
        }

        internal class UserConfiguration
        {
            public static void Seed(AppDbContext context)
            {
            }
        }

        internal class TagConfiguration
        {
            public static void Seed(AppDbContext context)
            {

            }
        }
    }
}
