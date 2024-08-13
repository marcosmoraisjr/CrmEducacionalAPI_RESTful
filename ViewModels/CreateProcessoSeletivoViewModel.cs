using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRMEducacional.CreateProcessoSeletivoViewModels
{
    public class CreateProcessoSeletivoViewModel // Singular, não plural
    {
        //[Required]
        //public int Id { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required]
        public DateTime DataInicio { get; set; } = DateTime.Now;

        [Required]
        public DateTime DataTermino { get; set; } = DateTime.Now;
    }
}
