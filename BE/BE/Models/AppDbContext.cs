// AppDbContext.cs
using Microsoft.EntityFrameworkCore;
using BE.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasKey(u => u.Id);
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .Property(u => u.IsGoogleLogin)
            .HasConversion<int>();

        base.OnModelCreating(modelBuilder);
    }

    public DbSet<Place> Places { get; set; }
    public DbSet<PlaceMedia> PlaceMedia { get; set; }
}

