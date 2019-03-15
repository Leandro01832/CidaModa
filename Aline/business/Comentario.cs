using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace business
{
    public class Comentario
    {
        [Key]
        public int IdComentario { get; set; }
        public string Comentar { get; set; }
        public string Email { get; set; }
        [Display(Name = "Codigo do produto")]
        public int produto_2 { get; set; }
        [ForeignKey("produto_2")]
        public virtual Produto Produto { get; set; }
    }
}
