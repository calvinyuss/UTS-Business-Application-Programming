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

                }
                finally
                {
                    connection.Close();
                }
            }
        }
    }
}
