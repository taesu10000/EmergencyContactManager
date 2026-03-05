using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class DBContext : DbContext
{

    public DBContext(DbContextOptions<DBContext> options) : base(options)
    {
    }

    public DbSet<Contact> Contacts { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Contact>().HasKey(x => new { x.email, x.tel });
    }
}
