namespace backend.Models
{
    public class Invoices
    {
        public Guid Id { get; set; } = Guid.NewGuid(); // Gera um novo GUID automaticamente
        public string? ProductName { get; set; }
        public decimal Price { get; set; }
    }
}
