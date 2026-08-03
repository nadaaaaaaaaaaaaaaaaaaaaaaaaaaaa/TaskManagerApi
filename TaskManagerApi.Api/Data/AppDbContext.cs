using Microsoft.EntityFrameworkCore;
using TaskManagerApi.Api.Models.Entities;

namespace TaskManagerApi.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<TaskItem> Tasks => Set<TaskItem>();
        public DbSet<User> Users => Set<User>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // User configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(u => u.Name)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(u => u.Email)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.HasIndex(u => u.Email)
                    .IsUnique();
            });

            // TaskItem configuration (extracted to dedicated class)
            modelBuilder.ApplyConfiguration(new TaskItemConfiguration());
        }
    }
}