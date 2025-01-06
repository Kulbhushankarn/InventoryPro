using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using InventoryPro.BM.ITF;
using InventoryPro.DL.ITF;
using InventoryPro.VO;

namespace InventoryPro.BM
{
    public class clsUserBM : IclsUserBM
    {
        private readonly IclsUserDL _userDL;

        public clsUserBM(IclsUserDL userDL)
        {
            _userDL = userDL ?? throw new ArgumentNullException(nameof(userDL));
        }

        public clsUser AuthenticateUser(SqlConnection connection, string username, string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentException("Username cannot be empty.", nameof(username));
            }

            if (string.IsNullOrWhiteSpace(passwordHash))
            {
                throw new ArgumentException("Password hash cannot be empty.", nameof(passwordHash));
            }

            try
            {
                return _userDL.GetUserByUsernameAndPassword(connection, username, passwordHash);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while authenticating the user.", ex);
            }
        }

        public void RegisterUser(SqlConnection connection, clsUser newUser)
        {
            if (newUser == null)
            {
                throw new ArgumentNullException(nameof(newUser), "User data cannot be null.");
            }

            if (string.IsNullOrWhiteSpace(newUser.Username))
            {
                throw new ArgumentException("Username cannot be empty.", nameof(newUser.Username));
            }

            if (string.IsNullOrWhiteSpace(newUser.PasswordHash))
            {
                throw new ArgumentException("Password hash cannot be empty.", nameof(newUser.PasswordHash));
            }

            if (string.IsNullOrWhiteSpace(newUser.Email))
            {
                throw new ArgumentException("Email cannot be empty.", nameof(newUser.Email));
            }

            if (string.IsNullOrWhiteSpace(newUser.Role))
            {
                throw new ArgumentException("Role cannot be empty.", nameof(newUser.Role));
            }

            try
            {
                _userDL.AddUser(connection, newUser);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while registering the user.", ex);
            }
        }

        public List<clsUser> GetAllUsers(SqlConnection connection)
        {
            try
            {
                return _userDL.GetAllUsers(connection);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving users.", ex);
            }
        }

        public void UpdateUser(SqlConnection connection, clsUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User data cannot be null.");
            }

            if (user.UserId <= 0)
            {
                throw new ArgumentException("Invalid user ID.", nameof(user.UserId));
            }

            try
            {
                _userDL.UpdateUser(connection, user);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the user.", ex);
            }
        }

        public void DeactivateUser(SqlConnection connection, int userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("Invalid user ID.", nameof(userId));
            }

            try
            {
                _userDL.DeleteUser(connection, userId);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deactivating the user.", ex);
            }
        }
    }
}
