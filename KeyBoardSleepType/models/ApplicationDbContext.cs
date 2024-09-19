using Microsoft.EntityFrameworkCore;

namespace KeyBoardSleepType.models
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Text> Texts { get; set; } = null!;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {  }
    }
}
