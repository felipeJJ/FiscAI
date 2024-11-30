using System;
using System.Collections.Generic;

namespace backend.Models
{
    public class Product
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid InvoiceId { get; set; }
        public required Invoice Invoice { get; set; } // Relacionamento com Invoice

        public string Nome { get; set; } = string.Empty;
        public string Item { get; set; } = string.Empty;
        public decimal Quantidade { get; set; }
        public string Unidade { get; set; } = string.Empty;
        public decimal Subtotal { get; set; }
        public decimal Total { get; set; }
    }
}