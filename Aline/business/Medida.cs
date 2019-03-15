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
        [Range(2,10, ErrorMessage ="A idade deve ser entre 2 e 10 anos")]
        public Int32? Idade { get; set; }
        public Int32? Quadril { get; set; }
        public Int32? Ombro { get; set; }
        public Int32? Torax { get; set; }
        public Int32? Altura { get; set; }
        public Int32? Cintura { get; set; }
        public Int32? Comprimento { get; set; }
        [Display(Name = "Codigo do produto")]
        public int produto_ { get; set; }
        [ForeignKey("produto_")]
        public virtual Produto Produto { get; set; }
        public Int32? pedido_ { get; set; }
        [ForeignKey("pedido_")]
        public virtual Pedido Pedido { get; set; }
        
    }
}
