using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Cashmere.Library.CashmereDataAccess
{
    public class DepositorContextFactory : IDesignTimeDbContextFactory<DepositorDBContext>
    {
        private readonly IConfiguration _configuration;
        public DepositorContextFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public DepositorDBContext CreateDbContext(string[] args)
        {
            string connectionString = _configuration.GetConnectionString("DepositorDatabase");

            Console.WriteLine($"DesignTimeDbContextFactory: using connection string = {connectionString}");

            var builder = new DbContextOptionsBuilder<DepositorDBContext>();

            builder.UseSqlServer(connectionString);

            return new DepositorDBContext(builder.Options);
        }
        public DbContextOptions<DepositorDBContext> Get()
        {

            var builder = new DbContextOptionsBuilder<DepositorDBContext>();
            DbContextConfigurer.Configure(builder, _configuration.GetConnectionString("DepositorDatabase"));

            return builder.Options;
        }
    }
    public class DbContextConfigurer
    {
        public static void Configure(
            DbContextOptionsBuilder<DepositorDBContext> builder,
            string connectionString)
        {
            builder.UseSqlServer(connectionString, options => options.EnableRetryOnFailure());
        }
    }
}
