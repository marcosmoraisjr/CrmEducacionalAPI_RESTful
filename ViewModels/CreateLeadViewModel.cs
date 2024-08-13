using System.ComponentModel.DataAnnotations;

namespace CRMEducacional.CreateLeadViewModels
{
    public class CreateLeadViewModel // Singular, não plural
    {
        //[Required]
        //public int Id { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Telefone { get; set; }

        [Required]
        public string CPF { get; set; }
    }
}
