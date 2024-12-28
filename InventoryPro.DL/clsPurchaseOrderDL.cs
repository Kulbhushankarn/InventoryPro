using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using InventoryPro.DL.ITF;
using InventoryPro.VO;

namespace InventoryPro.DL
{
    public class clsPurchaseOrderDL : IclsPurchaseOrderDL
    {
        public List<clsPurchaseOrder> GetAllPurchaseOrders(SqlConnection connection)
        {
            var purchaseOrders = new List<clsPurchaseOrder>();

            using (var command = new SqlCommand("SELECT * FROM PurchaseOrders", connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        purchaseOrders.Add(new clsPurchaseOrder
                        {
                            OrderId = reader.GetInt32(0),
                            SupplierName = reader.GetString(1),
                            OrderDate = reader.GetDateTime(2),
                            TotalCost = reader.GetDecimal(3),
                            Status = reader.GetString(4)
                        });
                    }
                }
            }

            return purchaseOrders;
        }

        public clsPurchaseOrder GetPurchaseOrderById(SqlConnection connection, int id)
        {
            clsPurchaseOrder order = null;

            using (var command = new SqlCommand("SELECT * FROM PurchaseOrders WHERE OrderId = @Id", connection))
            {
                command.Parameters.AddWithValue("@Id", id);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        order = new clsPurchaseOrder
                        {
                            OrderId = reader.GetInt32(0),
                            SupplierName = reader.GetString(1),
                            OrderDate = reader.GetDateTime(2),
                            TotalCost = reader.GetDecimal(3),
                            Status = reader.GetString(4)
                        };
                    }
                }
            }

            return order;
        }

        public void CreatePurchaseOrder(SqlConnection connection, clsPurchaseOrder order)
        {
            using (var command = new SqlCommand("INSERT INTO PurchaseOrders (SupplierName, OrderDate, TotalCost, Status) VALUES (@SupplierName, @OrderDate, @TotalCost, @Status)", connection))
            {
                command.Parameters.AddWithValue("@SupplierName", order.SupplierName);
                command.Parameters.AddWithValue("@OrderDate", order.OrderDate);
                command.Parameters.AddWithValue("@TotalCost", order.TotalCost);
                command.Parameters.AddWithValue("@Status", order.Status);

                command.ExecuteNonQuery();
            }
        }

        public void UpdatePurchaseOrder(SqlConnection connection, clsPurchaseOrder order)
        {
            using (var command = new SqlCommand("UPDATE PurchaseOrders SET SupplierName = @SupplierName, OrderDate = @OrderDate, TotalCost = @TotalCost, Status = @Status WHERE OrderId = @OrderId", connection))
            {
                command.Parameters.AddWithValue("@SupplierName", order.SupplierName);
                command.Parameters.AddWithValue("@OrderDate", order.OrderDate);
                command.Parameters.AddWithValue("@TotalCost", order.TotalCost);
                command.Parameters.AddWithValue("@Status", order.Status);
                command.Parameters.AddWithValue("@OrderId", order.OrderId);

                command.ExecuteNonQuery();
            }
        }

        public void DeletePurchaseOrder(SqlConnection connection, int id)
        {
            using (var command = new SqlCommand("DELETE FROM PurchaseOrders WHERE OrderId = @OrderId", connection))
            {
                command.Parameters.AddWithValue("@OrderId", id);
                command.ExecuteNonQuery();
            }
        }

        public List<clsPurchaseOrder> GetOrdersAboveThreshold(SqlConnection connection, decimal threshold)
        {
            var orders = new List<clsPurchaseOrder>();

            using (var command = new SqlCommand("SELECT * FROM PurchaseOrders WHERE TotalCost > @Threshold", connection))
            {
                command.Parameters.AddWithValue("@Threshold", threshold);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        orders.Add(new clsPurchaseOrder
                        {
                            OrderId = reader.GetInt32(0),
                            SupplierName = reader.GetString(1),
                            OrderDate = reader.GetDateTime(2),
                            TotalCost = reader.GetDecimal(3),
                            Status = reader.GetString(4)
                        });
                    }
                }
            }

            return orders;
        }
    }
}
