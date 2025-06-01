using Microsoft.EntityFrameworkCore;

namespace DatabaseMigrationProject.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<SampleEntity> SampleEntities { get; set; }
    }

    public class SampleEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
