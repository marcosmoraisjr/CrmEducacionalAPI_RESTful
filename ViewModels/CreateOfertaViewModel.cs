using System.ComponentModel.DataAnnotations;

namespace CRMEducacional.CreateOfertaViewModels
{
    public class CreateOfertaViewModel // Singular, não plural
    {
        //[Required]
        //public int Id { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required]
        public string Descricao { get; set; }

        [Required]
        public int VagasDisponiveis { get; set; }

    }
}
