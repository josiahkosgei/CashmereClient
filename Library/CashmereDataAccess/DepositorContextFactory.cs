using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Cashmere.Library.CashmereDataAccess
{
    public class DepositorContextFactory //: IDesignTimeDbContextFactory<DepositorDBContext>
    {
        private readonly IConfiguration _configuration;
        public DepositorContextFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public DepositorDBContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DepositorDBContext>();
            optionsBuilder.UseSqlServer(@"Data Source=.\;Initial Catalog=DepositorDatabase;Integrated Security=True",
                options => options.EnableRetryOnFailure());

            return new DepositorDBContext(optionsBuilder.Options);
        }
        public static DbContextOptions<DepositorDBContext> Get()
        {

            var builder = new DbContextOptionsBuilder<DepositorDBContext>();
            DbContextConfigurer.Configure(
                builder, @"Data Source=.\;Initial Catalog=DepositorDatabase;Integrated Security=True");

            return builder.Options;
        }
    }
    public class DbContextConfigurer
    {
        public static void Configure(
            DbContextOptionsBuilder<DepositorDBContext> builder, 
            string connectionString)
        {
            builder.UseSqlServer(connectionString,options => options.EnableRetryOnFailure());
        }
    }
}
