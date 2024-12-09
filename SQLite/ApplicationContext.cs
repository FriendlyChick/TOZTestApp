using Microsoft.EntityFrameworkCore;
namespace TOZTestApp.SQLite
{
    public class ApplicationContext : DbContext
    {
        public DbSet<MatchCalendarItemSQLite> MatchCalendarItemSQLite => Set<MatchCalendarItemSQLite>();
        public ApplicationContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=TozMatches.db");
        }
    }
}
