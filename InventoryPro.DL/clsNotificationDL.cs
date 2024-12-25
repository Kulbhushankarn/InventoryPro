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
    public class clsNotificationDL : IclsNotificationDL
    {
        private readonly string _connectionString;

        public clsNotificationDL(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void AddNotification(clsNotification notification)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("INSERT INTO Notifications (Message, Type, IsRead) VALUES (@Message, @Type, @IsRead)", connection))
                {
                    command.Parameters.AddWithValue("@Message", notification.Message);
                    command.Parameters.AddWithValue("@Type", notification.Type);
                    command.Parameters.AddWithValue("@IsRead", notification.IsRead);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public List<clsNotification> GetAllNotifications()
        {
            var notifications = new List<clsNotification>();
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("SELECT * FROM Notifications", connection))
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            notifications.Add(new clsNotification
                            {
                                NotificationId = reader.GetInt32(reader.GetOrdinal("NotificationId")),
                                Message = reader.GetString(reader.GetOrdinal("Message")),
                                Type = reader.GetString(reader.GetOrdinal("Type")),
                                IsRead = reader.GetBoolean(reader.GetOrdinal("IsRead"))
                            });
                        }
                    }
                }
            }
            return notifications;
        }
    }
}
