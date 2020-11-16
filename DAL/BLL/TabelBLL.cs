using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace DAL.BLL
{
    public class TableBLL : Connection
    {

        public DataSet ds;

        public TableBLL() : base()
        {
            fetchTabel();
        }

        public void fetchTabel()
        {
            string query = "SELECT * from tables";

            using (connection)
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(query, connection);

                    sqlAdapter = new SqlDataAdapter(command);

                    ds = new DataSet();

                    SqlCommandBuilder sqlCmdBuilder = new SqlCommandBuilder(sqlAdapter);

                    sqlAdapter.FillSchema(ds, SchemaType.Source, "table");

                    sqlAdapter.Fill(ds, "table");

                }catch(Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public void insert(string tableNumber)
        {

            if (isTableAlreadyExists(tableNumber)) throw new Exception("Table Number Already Exists");

            setConnection();
            string query = "INSERT INTO tables(table_number) VALUES(@table_number);SELECT SCOPE_IDENTITY()";
            using (connection)
            {
                try
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(query, connection);

                    sqlAdapter = new SqlDataAdapter(command);

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@table_number", tableNumber);

                    int id = Convert.ToInt32(command.ExecuteScalar());

                    DataRow row = ds.Tables["table"].NewRow();

                    row["id"] = id;
                    row["table_number"] = tableNumber;

                    ds.Tables["table"].Rows.Add(row);

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public void update(string id,string tableNumber)
        {
            try
            {
                DataRow row = ds.Tables["table"].Select("id="+id).FirstOrDefault();

                // check if table number sudah ada

                if (isTableAlreadyExists(tableNumber)) throw new Exception("Table Number Already Exists");

                row.BeginEdit();
                row["table_number"] = tableNumber;
                row.EndEdit();

                updateToDatabase();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
                
            }
        }

        public bool isTableAlreadyExists(string tableNumber)
        {
            DataRow row = ds.Tables["table"].Select("table_number="+tableNumber).FirstOrDefault();

            if (row == null) return false;

            return true; 
        }

        public void updateToDatabase()
        {
            setConnection();
            string query = "SELECT * from tables";

            using (connection)
            {
                try
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(query, connection);

                    sqlAdapter = new SqlDataAdapter(command);

                    SqlCommandBuilder sqlCmdBuilder = new SqlCommandBuilder(sqlAdapter);

                    sqlAdapter.Update(ds);

                }
                    catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        /*public DataTable getDataTabel()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("id", typeof(int));
            dt.Columns.Add("number", typeof(int));

            foreach (var table in fetchTabel())
            {
                var row = dt.NewRow();
                row["id"] = table.id;
                row["number"] = table.table_number;
                dt.Rows.Add(row);
            }

            return dt;
        }*/
    }
}
