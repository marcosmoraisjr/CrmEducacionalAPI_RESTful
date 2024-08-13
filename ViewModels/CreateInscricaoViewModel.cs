using System;
using System.ComponentModel.DataAnnotations;

namespace CRMEducacional.CreateInscricaoViewModels
{
    public class CreateInscricaoViewModel // Singular, não plural
    {
        //[Required]
        //public int Id { get; set; }

        public string NumeroDeInscricao { get; set; }
        public DateTime Data { get; set; } = DateTime.Now;
        public string Status { get; set; }
        public int LeadId { get; set; }
        public int ProcessoSeletivoId { get; set; }
        public int OfertaId { get; set; }
    }
}

