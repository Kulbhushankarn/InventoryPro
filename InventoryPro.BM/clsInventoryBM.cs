using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using InventoryPro.DL.ITF;
using InventoryPro.VO;
using InventoryPro.BM.ITF;

namespace InventoryPro.BM
{
    public class clsInventoryBM : IclsInventoryBM
    {
        private readonly IclsInventoryDL _inventoryDL;

        public clsInventoryBM(IclsInventoryDL inventoryDL)
        {
            _inventoryDL = inventoryDL;
        }

        public List<clsInventoryItem> GetAllInventoryItems(SqlConnection connection)
        {
            return _inventoryDL.GetAllInventoryItems(connection);
        }

        public clsInventoryItem GetInventoryItemById(SqlConnection connection, int itemId)
        {
            return _inventoryDL.GetAllInventoryItems(connection).Find(item => item.ItemId == itemId);
        }

        public void AddInventoryItem(SqlConnection connection, clsInventoryItem item)
        {
            _inventoryDL.AddInventoryItem(connection, item);
        }

        public void UpdateInventoryItem(SqlConnection connection, clsInventoryItem item)
        {
            _inventoryDL.UpdateInventoryItem(connection, item);
        }

        public void DeleteInventoryItem(SqlConnection connection, int itemId)
        {
            _inventoryDL.DeleteInventoryItem(connection, itemId);
        }

        public List<clsInventoryItem> GetItemsBelowReorderLevel(SqlConnection connection)
        {
            return _inventoryDL.GetItemsBelowReorderLevel(connection);
        }
    }
}
