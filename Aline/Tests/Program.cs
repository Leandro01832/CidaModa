using business;
using DataContextAline;
using System;

namespace Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            BD db = new BD();
            db.Database.Log = GravaLog;

            Produto p1 = new Vestido("Sapato");
            Produto p2 = new Daminha("Brinco");
            Produto p3 = new Saia("Colar");

            p1.Preco = 89.90m;
            p2.Preco = 59.90m;
            p3.Preco = 279.90m;

            p1.Estoque = 10;
            p2.Estoque = 10;
            p3.Estoque = 10;

            db.Produto.Add(p1);
            db.Produto.Add(p2);
            db.Produto.Add(p3);

            db.SaveChanges();

            Console.Read();
        }

        public static void GravaLog(string sql)
        {
            Console.WriteLine("Comando SQL: " + sql);
        }
    }
}
