using Microsoft.EntityFrameworkCore;
using BlackJackGameLogic.Models;

namespace BlackjackDataAccess.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<GameSession> GameSessions { get; set; }
        public DbSet<PerformanceMetric> PerformanceMetrics { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Customize the model if needed
            base.OnModelCreating(modelBuilder);
        }
    }
}
