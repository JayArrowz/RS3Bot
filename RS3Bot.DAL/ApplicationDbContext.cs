using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RS3Bot.Abstractions.Model;

namespace RS3Bot.DAL
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<string>, string>
    {
        private readonly ILoggerFactory _loggerFactory;

        public DbSet<EquipmentItem> EquipmentItems { get; set; }
        public DbSet<UserItem> UserItems { get; set; }
        public DbSet<SkillSet> SkillSets { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<CurrentTask> CurrentTasks { get; set; }
        public DbSet<ExpGain> CurrentTaskXpGains { get; set; }
        public DbSet<TaskItem> TaskItems { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options = null, ILoggerFactory loggerFactory = null)
            : base(options)
        {
            _loggerFactory = loggerFactory;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ApplicationUser>()
                .HasMany(t => t.Items)
                .WithOne(t => t.User)
                .HasForeignKey(t => t.UserId);

            builder.Entity<ApplicationUser>()
                .HasMany(t => t.Equipment)
                .WithOne(t => t.User)
                .HasForeignKey(t => t.UserId);
            
            builder.Entity<ApplicationUser>()
                .HasOne(t => t.CurrentTask)
                .WithOne(t => t.User)
                .HasForeignKey<CurrentTask>(t => t.UserId);

            builder.Entity<CurrentTask>()
                .HasMany(t => t.ExpGains)
                .WithOne(t => t.CurrentTask)
                .HasForeignKey(t => t.CurrentTaskId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ApplicationUser>()
                .HasOne(t => t.SkillSet)
                .WithOne(t => t.User)
                .HasForeignKey<SkillSet>(t => t.UserId);

            builder.Entity<CurrentTask>()
                .HasMany(t => t.Items)
                .WithOne(t => t.CurrentTask)
                .HasForeignKey(t => t.CurrentTaskId);
            base.OnModelCreating(builder);
        }
    }
}
