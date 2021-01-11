using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using RS3Bot.Cli;
using RS3Bot.DAL;

namespace RS3BotWeb.Cli
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var config = Program.GetConfiguration(args);

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseNpgsql(config.GetConnectionString("DefaultConnection"),
                 x => x.MigrationsAssembly(typeof(DesignTimeDbContextFactory)
                    .Assembly.GetName().Name));
            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
