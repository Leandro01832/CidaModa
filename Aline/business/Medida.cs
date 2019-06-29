using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace business
{
    [Table("Medida")]
    public class Medida
    {
        [Key]
        public int IdMedida { get; set; }
        public bool encomenda { get; set; }
        public Int32? Idade { get; set; }
        public Int32? Quadril { get; set; }
        public Int32? Ombro { get; set; }
        public Int32? Torax { get; set; }
        public Int32? Altura { get; set; }
        public Int32? Cintura { get; set; }
        public Int32? Comprimento { get; set; }
        public int itemPedido_ { get; set; }
        [ForeignKey("itemPedido_")]
        public virtual ItemPedido Item { get; set; }
        
    }
}
