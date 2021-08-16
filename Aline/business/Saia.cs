using System.ComponentModel.DataAnnotations.Schema;

namespace business
{
    [Table("Saia")]
    public class Saia : Produto
    {
        public Saia()
        {

        }

        public Saia(string nome) : base(nome)
        {
        }
    }
}
