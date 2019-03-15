using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace database
{
    class bdcomum
    {
        public SqlConnection conexao()
        {
            SqlConnection con = new SqlConnection("");
            con.Open();
            return con;
        }
    }
}
