using System;
using System.Collections.Generic;

namespace backend.Models
{
    public class Consumidor 
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string? Cpf_Cnpj { get; set; }
    }
}