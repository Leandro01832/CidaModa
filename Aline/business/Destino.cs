using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace business
{
   public class Destino
    {
        [Key]
        public int IdDestino { get; set; }
        public bool Ativacao { get; set; }
        public string Estados { get; set; }
    }
}
