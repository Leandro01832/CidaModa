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
   public class Imagem
    {
        [Key]
        public int IdImagem { get; set; }
        [DataType(DataType.ImageUrl)]
        public string ImagemProduto { get; set; }
        [NotMapped]
        public HttpPostedFileBase ImagemProdutoFile { get; set; }
        public int produtoImg_ { get; set; }
        [ForeignKey("produtoImg_")]
        public virtual Produto Produto { get; set; }
    }
}
