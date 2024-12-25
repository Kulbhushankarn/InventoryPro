using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventoryPro.VO;
using InventoryPro.DL.ITF;

namespace InventoryPro.DL
{
    public class clsPurchaseOrderDL : IclsPurchaseOrderDL
    {
        private readonly string _connectionString;

        public clsPurchaseOrderDL(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void CreatePurchaseOrder(clsPurchaseOrder order)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("INSERT INTO PurchaseOrders (OrderDate, SupplierName, TotalCost) VALUES (@OrderDate, @SupplierName, @TotalCost)", connection))
                {
                    command.Parameters.AddWithValue("@OrderDate", order.OrderDate);
                    command.Parameters.AddWithValue("@SupplierName", order.SupplierName);
                    command.Parameters.AddWithValue("@TotalCost", order.TotalCost);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public List<clsPurchaseOrder> GetAllPurchaseOrders()
        {
            var orders = new List<clsPurchaseOrder>();
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("SELECT * FROM PurchaseOrders", connection))
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            orders.Add(new clsPurchaseOrder
                            {
                                OrderId = reader.GetInt32(reader.GetOrdinal("OrderId")),
                                OrderDate = reader.GetDateTime(reader.GetOrdinal("OrderDate")),
                                SupplierName = reader.GetString(reader.GetOrdinal("SupplierName")),
                                TotalCost = reader.GetDecimal(reader.GetOrdinal("TotalCost"))
                            });
                        }
                    }
                }
            }
            return orders;
        }
    }
}
