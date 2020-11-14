using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace DAL.BLL
{
    public class AdminBLL : Connection
    {

        public bool loggedIn = false;

        public AdminBLL() : base()
        {
            
        }


        public void AttemptLogin(string username, string password)
        {
            using (this.connection)
            {
                string queryString = "select * from admins WHERE username=@username AND password=@password";
                SqlCommand command = new SqlCommand(queryString, this.connection);
                command.Parameters.AddWithValue("@username", username);
                command.Parameters.AddWithValue("@password", password);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        this.loggedIn = true;
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw new Exception(ex.Message);
                }
            }
        }

        public void test1()
        {
            /*using (DB_DATAEntities context = new DB_DATAEntities())
            {
                var query = from tables in context.tables select tables;

                foreach (var a in query)
                {
                    Console.WriteLine("\t{0}\t{1}", a.id, a.table_number);
                }
            }*/
        }
    }
}
