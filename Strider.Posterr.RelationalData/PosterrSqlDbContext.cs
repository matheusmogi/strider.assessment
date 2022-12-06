using Microsoft.EntityFrameworkCore;
using Strider.Posterr.Domain.Models;
using Strider.Posterr.RelationalData.Configurations;
using User = Strider.Posterr.Domain.Models.User;

namespace Strider.Posterr.RelationalData;

public class PosterrSqlDbContext : DbContext
{ 
    public PosterrSqlDbContext(DbContextOptions options) : base(options)
    {
        
    } 

    public DbSet<User> User { get; set; }
    public DbSet<Post> Post { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new PostConfiguration());
    }
}