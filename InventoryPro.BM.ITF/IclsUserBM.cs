using InventoryPro.VO;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace InventoryPro.BM.ITF
{
    public interface IclsUserBM
    {
        clsUser AuthenticateUser(SqlConnection connection, string username, string passwordHash, string role);
        void DeactivateUser(SqlConnection connection, int userId);
        List<clsUser> GetAllUsers(SqlConnection connection);
        void RegisterUser(SqlConnection connection, clsUser newUser);
        void UpdateUser(SqlConnection connection, clsUser user);
        //clsUser AuthenticateUser(SqlConnection connection, string username, string passwordHash);
    }
}