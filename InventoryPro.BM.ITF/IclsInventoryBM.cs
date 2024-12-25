using InventoryPro.VO;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace InventoryPro.BM.ITF
{
    public interface IclsInventoryBM
    {
        void AddInventoryItem(SqlConnection connection, clsInventoryItem item);
        void DeleteInventoryItem(SqlConnection connection, int itemId);
        List<clsInventoryItem> GetAllInventoryItems(SqlConnection connection);
        clsInventoryItem GetInventoryItemById(SqlConnection connection, int itemId);
        List<clsInventoryItem> GetItemsBelowReorderLevel(SqlConnection connection);
        void UpdateInventoryItem(SqlConnection connection, clsInventoryItem item);
    }
}