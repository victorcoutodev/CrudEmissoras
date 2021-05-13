using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CrudEmissoras.Models
{
    public class Emissora
    {
        public int Id { get; set; }

        [RegularExpression(@"^[A-Z]+[a-zA-Z\s]*$", ErrorMessage = "Proibido a inclusão de caracteres especiais no nome da emissora.")]
        [Required]
        public string Nome { get; set; }
    }
}
