using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using InventoryPro.VO;
using InventoryPro.DL.ITF;

namespace InventoryPro.DL
{
    public class clsUserDL : IclsUserDL
    {
        public List<clsUser> GetAllUsers(SqlConnection connection)
        {
            var users = new List<clsUser>();
            using (var command = new SqlCommand("SELECT * FROM Users", connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        users.Add(new clsUser
                        {
                            UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                            Username = reader.GetString(reader.GetOrdinal("Username")),
                            PasswordHash = reader.GetString(reader.GetOrdinal("PasswordHash")),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            Role = reader.GetString(reader.GetOrdinal("Role")),
                            IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
                        });
                    }
                }
            }
            return users;
        }

        public clsUser AuthenticateUser(SqlConnection connection, string username, string passwordHash, string role)
        {
            var query = "SELECT * FROM Users WHERE Username = @Username AND PasswordHash = @PasswordHash AND Role = @Role";
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@PasswordHash", passwordHash);
                command.Parameters.AddWithValue("@Role", role);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new clsUser
                        {
                            UserId = Convert.ToInt32(reader["UserId"]),
                            Username = reader["Username"].ToString(),
                            Role = reader["Role"].ToString()
                        };
                    }
                }
            }
            return null; // User not found
        }



        public clsUser GetUserByUsernameAndPassword(SqlConnection connection, string username, string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(passwordHash))
            {
                throw new ArgumentException("Username and password hash cannot be null or empty.");
            }

            try
            {
                string query = @"SELECT UserId, Username, PasswordHash, Email, IsActive 
                                 FROM Users 
                                 WHERE Username = @Username AND PasswordHash = @PasswordHash AND IsActive = 1";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@PasswordHash", passwordHash);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new clsUser
                            {
                                UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                                Username = reader.GetString(reader.GetOrdinal("Username")),
                                PasswordHash = reader.GetString(reader.GetOrdinal("PasswordHash")),
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
                            };
                        }
                        else
                        {
                            // Return null if no matching user is found
                            return null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the user by username and password.", ex);
            }
        }

        public void AddUser(SqlConnection connection, clsUser user)
        {
            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                if (user == null)
                {
                    throw new ArgumentNullException(nameof(user), "User data cannot be null.");
                }

                string query = @"INSERT INTO Users 
                                 (Username, PasswordHash, Email, Role, IsActive) 
                                 VALUES 
                                 (@Username, @PasswordHash, @Email, @Role, @IsActive)";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", user.Username ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@PasswordHash", user.PasswordHash ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Email", string.IsNullOrEmpty(user.Email) ? (object)DBNull.Value : user.Email);
                    command.Parameters.AddWithValue("@Role", string.IsNullOrEmpty(user.Role) ? (object)DBNull.Value : user.Role);
                    command.Parameters.AddWithValue("@IsActive", user.IsActive);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected <= 0)
                    {
                        throw new Exception("The user could not be added to the database.");
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception($"A database error occurred while adding the user: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while adding the user: {ex.Message}", ex);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        public void UpdateUser(SqlConnection connection, clsUser user)
        {
            using (var command = new SqlCommand("UPDATE Users SET Username = @Username, PasswordHash = @PasswordHash, Email = @Email, Role = @Role, IsActive = @IsActive WHERE UserId = @UserId", connection))
            {
                command.Parameters.AddWithValue("@UserId", user.UserId);
                command.Parameters.AddWithValue("@Username", user.Username);
                command.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
                command.Parameters.AddWithValue("@Email", user.Email);
                command.Parameters.AddWithValue("@Role", user.Role);
                command.Parameters.AddWithValue("@IsActive", user.IsActive);

                command.ExecuteNonQuery();
            }
        }

        public void DeleteUser(SqlConnection connection, int userId)
        {
            using (var command = new SqlCommand("UPDATE Users SET IsActive = 0 WHERE UserId = @UserId", connection))
            {
                command.Parameters.AddWithValue("@UserId", userId);
                command.ExecuteNonQuery();
            }
        }

        public clsUser GetUserById(SqlConnection connection, int userId)
        {
            clsUser user = null;
            using (var command = new SqlCommand("SELECT * FROM Users WHERE UserId = @UserId", connection))
            {
                command.Parameters.AddWithValue("@UserId", userId);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        user = new clsUser
                        {
                            UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                            Username = reader.GetString(reader.GetOrdinal("Username")),
                            PasswordHash = reader.GetString(reader.GetOrdinal("PasswordHash")),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            Role = reader.GetString(reader.GetOrdinal("Role")),
                            IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
                        };
                    }
                }
            }
            return user;
        }
    }
}
