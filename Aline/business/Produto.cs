using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace business
{
    [Table("Produto")]
    public class Produto
    {
        [Key]
        public int IdPrduto { get; set; }
        public int Estoque { get; set; }
        [DataType(DataType.ImageUrl)]
        public string Imagem { get; set; }
        public Double Preco { get; set; }
        [NotMapped]
        public HttpPostedFileBase ImagemFile { get; set; }
        public virtual ICollection<Pedido> Pedido { get; set; }
        public virtual ICollection<Medida> Medida { get; set; }
        public virtual ICollection<Comentario> Comentario { get; set; }

    }   
}
