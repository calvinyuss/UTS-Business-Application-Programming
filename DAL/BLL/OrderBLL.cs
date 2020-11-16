using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DAL.BLL
{
    public class OrderBLL :Connection
    {

        public int lastCreatedPaymentID;
        public OrderBLL() : base()
        {

        }

        public void createOrder(int tableID, List<OrderItem> items)
        {
            // 
            string orderQuery = "INSERT INTO orders(table_id, status) VALUES(@tableID, 'accepted'); SELECT SCOPE_IDENTITY()";

            string itemQuery = "INSERT INTO order_items(order_id, menu_id, quantity, unit_price) " +
                "VALUES (@order_id, @menu_id, @qty, @unit_price)";

            string paymentQuery = "INSERT INTO payments(order_id, amount) " +
                "VALUES (@order_id, @amount); SELECT SCOPE_IDENTITY()";

            decimal totalPayout = 0;

            using (connection)
            {
                try
                {
                    connection.Open();
                
                    SqlTransaction transaction = connection.BeginTransaction();

                    SqlCommand orderCommand = new SqlCommand(orderQuery, connection, transaction);

                    orderCommand.Parameters.AddWithValue("@tableID", tableID);

                    int orderID = Convert.ToInt32(orderCommand.ExecuteScalar());


                foreach (OrderItem item in items)
                {
                    SqlCommand itemCommand = new SqlCommand(itemQuery, connection, transaction);
                    itemCommand.Parameters.AddWithValue("@order_id", orderID);
                    itemCommand.Parameters.AddWithValue("@menu_id", item.menu_id);
                    itemCommand.Parameters.AddWithValue("@qty", item.quantity);
                    itemCommand.Parameters.AddWithValue("@unit_price", item.unit_price);
                    totalPayout += item.unit_price*item.quantity;
                    itemCommand.ExecuteNonQuery();
                }

                    SqlCommand paymentCommand = new SqlCommand(paymentQuery, connection, transaction);
                    paymentCommand.Parameters.AddWithValue("@order_id", orderID);
                    paymentCommand.Parameters.AddWithValue("@amount", totalPayout);

                    lastCreatedPaymentID = Convert.ToInt32(paymentCommand.ExecuteScalar());
                    transaction.Commit();

                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public DataSet getTopOrder()
        {
            string queryString =
            @"SELECT id as 'Menu ID', name, sale as 'Sold'
                FROM
                (
                    SELECT menu_id, COUNT(menu_id) as sale

                    FROM order_items    

                    GROUP BY menu_id
                ) AS weeklyMenuSales
                INNER JOIN menus
                on menus.id = weeklyMenuSales.menu_id
                WHERE menus.deleted_at IS NULL
                ORDER BY sale DESC";

            DataSet ds = new DataSet();
            using (connection)
            {
                // Create the Command and Parameter objects.
                SqlCommand command = new SqlCommand(queryString, connection);

                // Open the connection in a try/catch block.
                // Create and execute the DataReader, writing the result
                // set to the console window.
                try
                {
                    connection.Open();

                    sqlAdapter = new SqlDataAdapter(command);


                    SqlCommandBuilder sqlCmdBuilder = new SqlCommandBuilder(sqlAdapter);

                    sqlAdapter.FillSchema(ds, SchemaType.Source, "topOrder");

                    sqlAdapter.Fill(ds, "topOrder");

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                return ds;
            }
        }

        public DataSet getOrderItem()
        {
            string queryString =
            @"SELECT order_items.id as id, name, quantity, unit_price, create_at
                FROM order_items
                INNER JOIN menus
                ON menus.id = order_items.menu_id;";

            DataSet ds = new DataSet();
            using (connection)
            {
                // Create the Command and Parameter objects.
                SqlCommand command = new SqlCommand(queryString, connection);

                // Open the connection in a try/catch block.
                // Create and execute the DataReader, writing the result
                // set to the console window.
                try
                {
                    connection.Open();

                    sqlAdapter = new SqlDataAdapter(command);


                    SqlCommandBuilder sqlCmdBuilder = new SqlCommandBuilder(sqlAdapter);

                    sqlAdapter.FillSchema(ds, SchemaType.Source, "orderItem");

                    sqlAdapter.Fill(ds, "orderItem");

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                return ds;
            }
        }

        public DataSet getPaymentOrder()
        {
            string queryString =
            @"SELECT order_id, payments.id as 'payment id', table_number as 'table number', amount, status, created_at
                FROM payments
                INNER JOIN orders
                ON payments.order_id = orders.id
                INNER JOIN tables
                ON tables.id = orders.table_id";

            DataSet ds = new DataSet();
            using (connection)
            {
                // Create the Command and Parameter objects.
                SqlCommand command = new SqlCommand(queryString, connection);

                // Open the connection in a try/catch block.
                // Create and execute the DataReader, writing the result
                // set to the console window.
                try
                {
                    connection.Open();

                    sqlAdapter = new SqlDataAdapter(command);


                    SqlCommandBuilder sqlCmdBuilder = new SqlCommandBuilder(sqlAdapter);

                    sqlAdapter.FillSchema(ds, SchemaType.Source, "paymentOrder");
                   
                    sqlAdapter.Fill(ds, "paymentOrder");

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                return ds;
            }
        }
    }
}
