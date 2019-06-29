using business;
using PagSeguro;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataContextAline
{
   public class BD : DbContext
    {
        public BD() : base("DefaultConnection")
        {
           // Database.SetInitializer<BD>(null);
        }

        //public override int SaveChanges()
        //{
        //    try
        //    {
        //        return base.SaveChanges();
        //    }
        //    catch (DbEntityValidationException e)
        //    {
        //        foreach (var eve in e.EntityValidationErrors)
        //        {
        //            Console.WriteLine("Entidade do tipo \"{0}\" no estado \"{1}\" tem os seguintes erros de validação:",
        //                eve.Entry.Entity.GetType().Name, eve.Entry.State);
        //            foreach (var ve in eve.ValidationErrors)
        //            {
        //                Console.WriteLine("- Property: \"{0}\", Erro: \"{1}\"",
        //                    ve.PropertyName, ve.ErrorMessage);
        //            }
        //        }
        //        throw;
        //    }
        //}

        public DbSet<Cliente> Cliente { get; set; }
        public DbSet<Daminha> Daminha { get; set; }
        public DbSet<Saia> Saia { get; set; }
        public DbSet<Vestido> Vestido { get; set; }
        public DbSet<Comentario> Comentario { get; set; }
        public DbSet<Telefone> Telefone { get; set; }
        public DbSet<Endereco> Endereco { get; set; }
        public DbSet<Destino> Destino { get; set; }
        public DbSet<Produto> Produto { get; set; }
        public DbSet<Imagem> Imagem { get; set; }
        public DbSet<Pedido> Pedido { get; set; }
        public DbSet<ItemPedido> ItemPedido { get; set; }
        public DbSet<Medida> Medida { get; set; }
        public DbSet<Dados> Dados { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

        }
       
    }
}
