using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace business
{
   public class ItemPedido
    {
        [Key]
        public int IdItem { get; set; }
        [Required(ErrorMessage = "Este campo precisa ser preenchido")]
        public int Quantidade { get; set; }
        public int pedido_ { get; set; }
        [ForeignKey("pedido_")]
        public virtual Pedido pedido { get; set; }
        public int produto_ { get; set; }
        [ForeignKey("produto_")]
        public virtual Produto produto { get; set; }
        
    }
}
