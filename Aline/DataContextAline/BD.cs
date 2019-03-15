using business;
using PagSeguro;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataContextAline
{
   public class BD : DbContext
    {
        public BD() : base("DefaultConnection")
        {

        }

        public DbSet<Cliente> Cliente { get; set; }
        public DbSet<Daminha> Daminha { get; set; }
        public DbSet<Saia> Saia { get; set; }
        public DbSet<Vestido> Vestido { get; set; }
        public DbSet<Comentario> Comentario { get; set; }
        public DbSet<Telefone> Telefone { get; set; }
        public DbSet<Endereco> Endereco { get; set; }
        public DbSet<Produto> Produto { get; set; }
        public DbSet<Pedido> Pedido { get; set; }
        public DbSet<Medida> Medida { get; set; }
        public DbSet<Dados> Dados { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            // modelBuilder.Conventions.Remove<PrimaryKeyNameForeignKeyDiscoveryConvention>();
            //  modelBuilder.Entity<Celula>().HasKey(c => c.Id).HasEntitySetName("Celulaid");

        }
    }
}
