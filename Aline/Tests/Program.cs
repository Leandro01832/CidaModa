using business;
using DataContextAline;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            BD db = new BD();
            db.Database.Log = GravaLog;

            var cli = db.Cliente.Find(1);

            Produto p1 = db.Produto.Find(1);
            Produto p2 = db.Produto.Find(2);
            Produto p3 = db.Produto.Find(3);

            var ped = new Pedido
            {
                Datapedido = DateTime.Now,
                Status = "",
                Endereco = new Endereco
                {
                    Bairro = "",
                    Cep = 36774016,
                    Cidade = "Cataguases",
                    Estado = "Mg",
                    Numero = 117,
                    Rua = "Rua Joaquim Augusto de Almeida",
                    ValorFrete = ""
                },
                Itens = new List<ItemPedido>
                  {
                      new ItemPedido{ produto_ = p1.IdPrduto, Quantidade = 3, produto = p1 },
                      new ItemPedido{ produto_ = p1.IdPrduto, Quantidade = 3, produto = p2 },
                      new ItemPedido{ produto_ = p1.IdPrduto, Quantidade = 3, produto = p3 }
                  },
                Cliente = cli,
                ClienteId = cli.IdCliente.ToString()


            };
            double valor = 0;
            var valores = ped.Itens.Select(i => i.produto.Preco * i.Quantidade).ToList();
            foreach (var item in valores)
                valor += item;
            ped.ValorPedido += double.Parse(valor.ToString("F2"));

            db.Pedido.Add(ped);

            db.SaveChanges();

            Console.WriteLine("Ok");

            Console.Read();

        }

        public static void GravaLog(string sql)
        {
            Console.WriteLine("Comando SQL: " + sql);
        }
    }
}
