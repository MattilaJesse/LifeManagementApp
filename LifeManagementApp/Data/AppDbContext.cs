using LifeManagementApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace LifeManagementApp.Data;

public class AppDbContext : DbContext
{
    public DbSet<TodoItem> TodoItems => Set<TodoItem>();
    public DbSet<Category> Categories => Set<Category>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Määritellään 1-N relaatio
        modelBuilder.Entity<TodoItem>()
            .HasOne(t => t.Category)
            .WithMany(c => c.TodoItems)
            .HasForeignKey(t => t.CategoryId);

        // Valmiit oletuskategoriat tietokantaa luotaessa
        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Yleinen" },
            new Category { Id = 2, Name = "Koulu" },
            new Category { Id = 3, Name = "Koti" },
            new Category { Id = 4, Name = "Vapaa-aika" }
        );
    }
}