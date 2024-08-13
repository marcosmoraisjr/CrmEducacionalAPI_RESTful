using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRMEducacional.Models
{
    public class Inscricao
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string NumeroDeInscricao { get; set; }
        public DateTime Data { get; set; } = DateTime.Now;
        public string Status { get; set; }
        public int LeadId { get; set; }
        public int ProcessoSeletivoId { get; set; }
        public int OfertaId { get; set; }


        // Propriedade de Navegação
        public Lead Lead { get; set; }
        public Oferta Oferta { get; set; }
    }
}
