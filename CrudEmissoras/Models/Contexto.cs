using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrudEmissoras.Models
{
    public class Contexto : DbContext
    {
        public DbSet<Emissora> Emissoras { get; set; }

        public DbSet<Audiencia> Audiencias { get; set; }

        public Contexto(DbContextOptions<Contexto> opcoes) : base(opcoes)
        {

        }
    }
}
