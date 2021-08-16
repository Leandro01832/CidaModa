using System.ComponentModel.DataAnnotations.Schema;

namespace business
{
    [Table("Daminha")]
    public class Daminha : Produto
    {
        public Daminha()
        {

        }

        public Daminha(string nome) : base(nome)
        {
        }
    }
}
