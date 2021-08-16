using System.ComponentModel.DataAnnotations.Schema;

namespace business
{
    [Table("Vestido")]
    public class Vestido : Produto
    {
        public Vestido()
        {

        }

        public Vestido(string nome) : base(nome)
        {
        }
    }
}
