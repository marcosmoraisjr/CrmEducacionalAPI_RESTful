using CRMEducacional.Models; // Importa o namespace onde estão definidas as classes
using Microsoft.EntityFrameworkCore;

namespace CRMEducacional.Data
{
    public class AppDbContext : DbContext
    {

        public DbSet<Lead> Leads { get; set; }
        public DbSet<Oferta> Ofertas { get; set; }
        public DbSet<Inscricao> Inscricoes { get; set; }
        public DbSet<ProcessoSeletivo> ProcessosSeletivos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=database.db");
        }

    }
}