using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using RS3Bot.DAL;

namespace RS3BotWeb.Server
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=aspnet-RS3BotWeb.Server-02FE1124-FB28-4813-8FAA-DF3D729EA5F6;Trusted_Connection=True;MultipleActiveResultSets=true",
                 x => x.MigrationsAssembly(typeof(DesignTimeDbContextFactory)
                    .Assembly.GetName().Name));
            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
