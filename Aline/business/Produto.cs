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
    public abstract class Produto
    {
        [Key]     
        [Display(Name ="Codigo do produto")]   
        public int IdPrduto { get; set; }
        public string Nome { get; private set; }
        [DataType(DataType.ImageUrl)]
        public string Imagem { get; set; }
        [NotMapped]
        public HttpPostedFileBase ImagemFile { get; set; }
        public int Estoque { get; set; }
        public decimal Preco { get; set; }
        public virtual ICollection<Comentario> Comentario { get; set; }
        public virtual ICollection<Imagem> Imagens { get; set; }
        public virtual ICollection<Medida> Medida { get; set; }

        public Produto()
        {

        }

        public Produto(string nome)
        {
            this.Nome = nome;
        }
    }   
}
