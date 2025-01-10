using InventoryPro.VO;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace InventoryPro.DL.ITF
{
    public interface IclsUserDL
    {
        void AddUser(SqlConnection connection, clsUser user);
        void DeleteUser(SqlConnection connection, int userId);
        List<clsUser> GetAllUsers(SqlConnection connection);
        clsUser GetUserById(SqlConnection connection, int userId);
        clsUser GetUserByUsernameAndPassword(SqlConnection connection, string username, string passwordHash);
        void UpdateUser(SqlConnection connection, clsUser user);
        clsUser AuthenticateUser(SqlConnection connection, string username, string role);
    }
}