using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CrudEmissoras.Models
{
    public class Audiencia
    {
        public int Id { get; set; }

        [Required]
        public int Pontos_audiencia { get; set; }

        [Required]
        public DateTime Data_hora_audiencia { get; set; }

        [Required]
        public Emissora Emissora_audiencia { get; set; }
    }
}
