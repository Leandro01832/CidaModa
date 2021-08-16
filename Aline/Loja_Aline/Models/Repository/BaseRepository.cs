using DataContextAline;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Loja_Aline.Models.Repository
{
    public class BaseRepository<T> where T : class
    {
        protected readonly BD contexto;
        protected readonly DbSet<T> dbSet;

        public BaseRepository(BD contexto)
        {
            this.contexto = contexto;
            dbSet = contexto.Set<T>();
        }
    }
}