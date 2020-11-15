using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace DAL
{
    public class Connection
    {
        public string connectionString { get; set; }
        public SqlConnection connection { get; set; }
        public SqlDataAdapter sqlAdapter { get; set; }

        public Connection()
        {
            setConnection();
        }

        public void setConnection()
        {
            this.connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DB_DATA"].ConnectionString;
            this.connection = new SqlConnection(this.connectionString);
        }

    }
}
