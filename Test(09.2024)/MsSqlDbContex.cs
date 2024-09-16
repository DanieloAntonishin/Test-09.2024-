using Microsoft.EntityFrameworkCore;
using Task.Models;

namespace Task;

public class MsSqlDbContext : DbContext
{
    public DbSet<Hall> HallEntities { get; set; }                       
    public DbSet<HallAddon> AddonsEntities { get; set; }
    public DbSet<RentTime> RentTimeEntities { get; set; }
    public MsSqlDbContext(DbContextOptions<MsSqlDbContext> options) : base(options) { }
    protected override void OnModelCreating(ModelBuilder modelBuilder)  // Add new Entitys "Table" to DB
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Hall>();
        modelBuilder.Entity<HallAddon>();
        modelBuilder.Entity<RentTime>();
    }
}
