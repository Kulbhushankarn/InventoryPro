using InventoryPro.VO;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace InventoryPro.DL.ITF
{
    public interface IclsInventoryDL
    {
        void AddInventoryItem(SqlConnection connection, clsInventoryItem item);
        void DeleteInventoryItem(SqlConnection connection, int itemId);
        List<clsInventoryItem> GetAllInventoryItems(SqlConnection connection);
        List<clsInventoryItem> GetItemsBelowReorderLevel(SqlConnection connection);
        void UpdateInventoryItem(SqlConnection connection, clsInventoryItem item);
    }
}