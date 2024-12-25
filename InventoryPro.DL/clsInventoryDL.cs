using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using InventoryPro.VO;
using InventoryPro.DL.ITF;

namespace InventoryPro.DL
{
    public class clsInventoryDL : IclsInventoryDL
    {
        public List<clsInventoryItem> GetAllInventoryItems(SqlConnection connection)
        {
            var inventoryItems = new List<clsInventoryItem>();
            using (var command = new SqlCommand("SELECT * FROM InventoryItems", connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        inventoryItems.Add(new clsInventoryItem
                        {
                            ItemId = reader.GetInt32(reader.GetOrdinal("ItemId")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Category = reader.GetString(reader.GetOrdinal("Category")),
                            Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                            Unit = reader.GetString(reader.GetOrdinal("Unit")),
                            Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                            ReorderLevel = reader.GetInt32(reader.GetOrdinal("ReorderLevel")),
                            IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
                        });
                    }
                }
            }
            return inventoryItems;
        }

        public void AddInventoryItem(SqlConnection connection, clsInventoryItem item)
        {
            try
            {
                // Ensure the connection is open
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                // Validate the input data
                if (item == null)
                {
                    throw new ArgumentNullException(nameof(item), "Item data cannot be null.");
                }

                string query = @"INSERT INTO InventoryItems 
                         (Name, Category, Quantity, Unit, Price, ReorderLevel, IsActive) 
                         VALUES 
                         (@Name, @Category, @Quantity, @Unit, @Price, @ReorderLevel, @IsActive)";

                using (var command = new SqlCommand(query, connection))
                {
                    // Add parameters with null-checks
                    command.Parameters.AddWithValue("@Name", item.Name ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Category", string.IsNullOrEmpty(item.Category) ? (object)DBNull.Value : item.Category);
                    command.Parameters.AddWithValue("@Quantity", item.Quantity > 0 ? item.Quantity : (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Unit", string.IsNullOrEmpty(item.Unit) ? (object)DBNull.Value : item.Unit);
                    command.Parameters.AddWithValue("@Price", item.Price > 0 ? item.Price : (object)DBNull.Value);
                    command.Parameters.AddWithValue("@ReorderLevel", item.ReorderLevel >= 0 ? item.ReorderLevel : (object)DBNull.Value);
                    command.Parameters.AddWithValue("@IsActive", item.IsActive);

                    // Execute the command
                    int rowsAffected = command.ExecuteNonQuery();

                    // Check if the row was inserted
                    if (rowsAffected <= 0)
                    {
                        throw new Exception("The inventory item could not be added to the database.");
                    }
                }
            }
            catch (SqlException ex)
            {
                // Log or handle SQL exceptions
                throw new Exception($"A database error occurred while adding the inventory item: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                // Log or handle general exceptions
                throw new Exception($"An error occurred while adding the inventory item: {ex.Message}", ex);
            }
            finally
            {
                // Ensure the connection is closed
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }


        public void UpdateInventoryItem(SqlConnection connection, clsInventoryItem item)
        {
            using (var command = new SqlCommand("UPDATE InventoryItems SET Name = @Name, Category = @Category, Quantity = @Quantity, Unit = @Unit, Price = @Price, ReorderLevel = @ReorderLevel WHERE ItemId = @ItemId", connection))
            {
                command.Parameters.AddWithValue("@ItemId", item.ItemId);
                command.Parameters.AddWithValue("@Name", item.Name);
                command.Parameters.AddWithValue("@Category", item.Category);
                command.Parameters.AddWithValue("@Quantity", item.Quantity);
                command.Parameters.AddWithValue("@Unit", item.Unit);
                command.Parameters.AddWithValue("@Price", item.Price);
                command.Parameters.AddWithValue("@ReorderLevel", item.ReorderLevel);
                command.ExecuteNonQuery();
            }
        }

        public void DeleteInventoryItem(SqlConnection connection, int itemId)
        {
            using (var command = new SqlCommand("UPDATE InventoryItems SET IsActive = 0 WHERE ItemId = @ItemId", connection))
            {
                command.Parameters.AddWithValue("@ItemId", itemId);
                command.ExecuteNonQuery();
            }
        }

        public List<clsInventoryItem> GetItemsBelowReorderLevel(SqlConnection connection)
        {
            var inventoryItems = new List<clsInventoryItem>();
            using (var command = new SqlCommand("SELECT * FROM InventoryItems WHERE Quantity <= ReorderLevel AND IsActive = 1", connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        inventoryItems.Add(new clsInventoryItem
                        {
                            ItemId = reader.GetInt32(reader.GetOrdinal("ItemId")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Category = reader.GetString(reader.GetOrdinal("Category")),
                            Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                            Unit = reader.GetString(reader.GetOrdinal("Unit")),
                            Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                            ReorderLevel = reader.GetInt32(reader.GetOrdinal("ReorderLevel")),
                            IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
                        });
                    }
                }
            }
            return inventoryItems;
        }
    }
}
