using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRMEducacional.Models
{
    public class ProcessoSeletivo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Nome { get; set; }
        public DateTime DataInicio { get; set; } = DateTime.Now;
        public DateTime DataTermino { get; set; } = DateTime.Now.AddYears(1);
    }
}
