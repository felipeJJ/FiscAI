using System;
using System.Collections.Generic;

namespace backend.Models{
    public class Invoice
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Status { get; set; } = "concluido"; // concluido, parcial, cancelado, invalido
        public string Chave { get; set; } = string.Empty; // Chave única
        public bool NotaValida { get; set; } = true; // Nota válida
        public DateTime DataEmissao { get; set; }
        public int Numero { get; set; } // Número da nota
        public int Serie { get; set; }
        public decimal Total { get; set; } // Valor total da nota
        public Guid? EmitenteId { get; set; }
        public required Emitente Emitente { get; set; } // Relacionamento com Emitente
        public Guid ConsumidorId { get; set; }
        public Consumidor? Consumidor { get; set; } // Relacionamento opcional com Consumidor
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
