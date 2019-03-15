﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace business
{
    [Table("Cliente")]
    public class Cliente
    {    
        [Key]    
        public int IdCliente { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Cpf { get; set; }
        public virtual Telefone Telefone { get; set; }
        public virtual ICollection<Pedido> Pedidos { get; set; }
    }
}