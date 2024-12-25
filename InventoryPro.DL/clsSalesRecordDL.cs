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
    public class clsSalesRecordDL : IclsSalesRecordDL
    {
        private readonly string _connectionString;

        public clsSalesRecordDL(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void AddSalesRecord(clsSalesRecord record)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("INSERT INTO SalesRecords (ItemId, QuantitySold, SaleDate, SaleAmount) VALUES (@ItemId, @QuantitySold, @SaleDate, @SaleAmount)", connection))
                {
                    command.Parameters.AddWithValue("@ItemId", record.ItemId);
                    command.Parameters.AddWithValue("@QuantitySold", record.QuantitySold);
                    command.Parameters.AddWithValue("@SaleDate", record.SaleDate);
                    command.Parameters.AddWithValue("@SaleAmount", record.SaleAmount);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public List<clsSalesRecord> GetAllSalesRecords()
        {
            var records = new List<clsSalesRecord>();
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("SELECT * FROM SalesRecords", connection))
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            records.Add(new clsSalesRecord

                            {
                                SaleId = reader.GetInt32(reader.GetOrdinal("SaleId")),
                                ItemId = reader.GetInt32(reader.GetOrdinal("ItemId")),
                                QuantitySold = reader.GetInt32(reader.GetOrdinal("QuantitySold")),
                                SaleDate = reader.GetDateTime(reader.GetOrdinal("SaleDate")),
                                SaleAmount = reader.GetDecimal(reader.GetOrdinal("SaleAmount"))
                            });
                        }
                    }
                }
            }
            return records;
        }
    }
}