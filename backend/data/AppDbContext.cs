using backend.Models;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    // Construtor
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // DbSets para cada entidade
    public DbSet<Invoice> Invoices { get; set; } = null!;
    public DbSet<Emitente> Emitentes { get; set; } = null!;
    public DbSet<Consumidor> Consumidores { get; set; } = null!;
    public DbSet<Product> Products { get; set; } = null!;

    // Configurações do modelo
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configurar a chave primária para Invoice
        modelBuilder.Entity<Invoice>()
            .HasKey(i => i.Id);

        // Configurar a chave primária para Emitente
        modelBuilder.Entity<Emitente>()
            .HasKey(e => e.Id);

        // Configurar a chave primária para Consumidor
        modelBuilder.Entity<Consumidor>()
            .HasKey(c => c.Id);

        // Configurar a chave primária para Product
        modelBuilder.Entity<Product>()
            .HasKey(p => p.Id);

        // Relacionamento entre Invoice e Emitente
        modelBuilder.Entity<Invoice>()
            .HasOne(i => i.Emitente)
            .WithMany(e => e.Invoices)
            .HasForeignKey(i => i.EmitenteId);

        // Relacionamento entre Invoice e Consumidor
        modelBuilder.Entity<Invoice>()
            .HasOne(i => i.Consumidor)
            .WithMany()
            .HasForeignKey(i => i.ConsumidorId);

        // Relacionamento entre Product e Invoice
        modelBuilder.Entity<Product>()
            .HasOne(p => p.Invoice)
            .WithMany(i => i.Products)
            .HasForeignKey(p => p.InvoiceId);

        base.OnModelCreating(modelBuilder);
    }
}
