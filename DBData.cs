using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Vitta_Testing_Task
{
    public class DBData
    {
        public string ConnectionString { get; }

        public DBData(string username, string password)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            ConnectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=Testing_Task_VITTA;User ID=" + username +";Password=" + password;
        }

        public static decimal Amount(SqlConnection sqlConnection)
        {
            string queryString = "SELECT amount FROM dbo.Money WHERE moneyId = (SELECT max(moneyId) FROM dbo.Money)";
            SqlCommand sqlCommand = new SqlCommand(queryString, sqlConnection);
            using (SqlDataReader reader = sqlCommand.ExecuteReader())
            {
                reader.Read();
                return (decimal)reader[0];
            }
        }

        public static List<Order> DefaultView(SqlConnection sqlConnection)
        {
            List<Order> orders = new List<Order>();
            string queryString = "SELECT orderId, date, sum, toPayment FROM dbo.Orders WHERE toPayment > 0";
            SqlCommand sqlCommand = new SqlCommand(queryString, sqlConnection);
            using (SqlDataReader reader = sqlCommand.ExecuteReader())
            {
                while(reader.Read())
                {
                    orders.Add(new Order((int)reader[0], (DateTime)reader[1], (decimal)reader[2], (decimal)reader[3]));
                }
            }
            return orders;
        }

        public static void Pay(SqlConnection sqlConnection, Order order, decimal value)
        {
            string queryString = "BEGIN TRANSACTION DECLARE @TOPAY MONEY; UPDATE dbo.Orders SET toPayment = toPayment - "+ value +" WHERE orderId = "+ order.id + "; IF(@@ERROR <> 0) ROLLBACK IF(@TOPAY < 0) ROLLBACK COMMIT TRANSACTION";
            SqlCommand sqlCommand = new SqlCommand(queryString, sqlConnection);
            try
            {
                sqlCommand.ExecuteNonQuery();
            }
            catch
            {
                throw;
            }
        }

        public static List<Payment> PaymentView(SqlConnection sqlConnection)
        {
            List<Payment> payments = new List<Payment>();
            string queryString = "SELECT * FROM dbo.Payment";
            SqlCommand sqlCommand = new SqlCommand(queryString, sqlConnection);
            using (SqlDataReader reader = sqlCommand.ExecuteReader())
            {
                while (reader.Read())
                {
                    payments.Add(new Payment((int)reader[0], (int)reader[1], (decimal)reader[2]));
                }
            }
            return payments;
        }
    }
}
