using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace RS3Bot.DAL
{
    public class DbContextFactory : IContextFactory
    {
        private readonly DbContextOptions<ApplicationDbContext> _RosteringContextOptions;
        private readonly ILoggerFactory _loggerFactory;

        public DbContextFactory(ILoggerFactory loggerFactory, DbContextOptions<ApplicationDbContext> RosteringContextOptions)
        {
            _loggerFactory = loggerFactory;
            _RosteringContextOptions = RosteringContextOptions;
        }

        public ApplicationDbContext Create()
        {
            return new ApplicationDbContext(_RosteringContextOptions, _loggerFactory);
        }
    }
}
