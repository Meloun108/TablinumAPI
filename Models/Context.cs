using Microsoft.EntityFrameworkCore;

namespace tablinumAPI.Models
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options)
            : base(options)
        {
        }

        public DbSet<Document> Documents { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Initio> Initio { get; set; }
        public DbSet<Role> Roles { get; set; }
    }
}