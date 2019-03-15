using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace business
{
    [Table("Telefone")]
    public class Telefone
    {
        [Key, ForeignKey("Cliente")]
        public int IdTelefone { get; set; }
        public string DDD_Celular { get; set; }
        public string Celular { get; set; }
        public string DDD_Telefone { get; set; }
        public string Fone { get; set; }
        public virtual Cliente Cliente { get; set; }
    }
}
