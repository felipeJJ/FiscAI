using System;

namespace backend.Models {
    public class Emitente
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Cnpj { get; set; } = string.Empty;
        public string RazaoSocial { get; set; } = string.Empty;
        public ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
    }
}
