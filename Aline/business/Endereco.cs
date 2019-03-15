using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace business
{
    [Table("Endereco")]
    public class Endereco
    {
        [Key, ForeignKey("Pedido")]
        public int IdEndereco { get; set; }
        public string Estado { get; set; }
        public string Cidade { get; set; }
        public string Bairro { get; set; }
        public string Rua { get; set; }
        public long Numero { get; set; }
        public long Cep { get; set; }
        public string ValorFrete { get; set; }
        public virtual Pedido Pedido { get; set; }
    }
}
