using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        public int Emissora_audiencia_id { get; set; }

        public string NomeEmissora { get; set; }
    }
}
