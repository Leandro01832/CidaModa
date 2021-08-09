using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace business
{
    [Table("Pedido")]
    public class Pedido
    {
        [Key]
        public int IdPedido { get; set; }
        public double ValorPedido { get; set; }
        public virtual List<ItemPedido> Itens { get; set; }
        public virtual Endereco Endereco { get; set; }
        public virtual DateTime Datapedido { get; set; }
        [Required]
        public virtual Cliente Cliente { get; set; }
        [Required]
        public string ClienteId { get; set; }
        public string Status { get; set; }
    }
}
