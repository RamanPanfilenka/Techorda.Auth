using DA.Entities;
using Microsoft.EntityFrameworkCore;

namespace DA
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) 
            : base(options) { }
    }
}
