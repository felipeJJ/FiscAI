namespace backend.Models
{
    public class Notas
    {
        public int Id { get; set; }
        public required string Produto { get; set; }
        public decimal Valor { get; set; }
    }
}