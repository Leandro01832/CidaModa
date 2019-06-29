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
        [Display(Name ="Codigo do produto")]   
        public int IdPrduto { get; set; }
        [DataType(DataType.ImageUrl)]
        public string Imagem { get; set; }
        [NotMapped]
        public HttpPostedFileBase ImagemFile { get; set; }
        public int Estoque { get; set; }
        public Double Preco { get; set; }
        public virtual ICollection<Comentario> Comentario { get; set; }
        public virtual ICollection<Imagem> Imagens { get; set; }

    }   
}
