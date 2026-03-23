using Microsoft.EntityFrameworkCore;
using Qowaiv.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;

namespace Specs.Models;

public sealed class ModelWithEmail
{
    [Key]
    public Guid Id { get; init; } = Guid.NewGuid();

    public EmailAddress Email { get; init; }
}

public class ModelWithEmailContext(Action<DbContextOptionsBuilder> configure) : DbContext()
{
    public DbSet<ModelWithEmail> Models { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<ModelWithEmail>()
            .SvoProperty(m => m.Email);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        configure(optionsBuilder);
    }
}
