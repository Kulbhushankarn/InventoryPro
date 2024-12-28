using InventoryPro.VO;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace InventoryPro.BM.ITF
{
    public interface IclsPurchaseOrderBM
    {
        void CreatePurchaseOrder(SqlConnection connection, clsPurchaseOrder order);
        void DeletePurchaseOrder(SqlConnection connection, int id);
        List<clsPurchaseOrder> GetAllPurchaseOrders(SqlConnection connection);
        List<clsPurchaseOrder> GetOrdersAboveThreshold(SqlConnection connection, decimal threshold);
        clsPurchaseOrder GetPurchaseOrderById(SqlConnection connection, int id);
        void UpdatePurchaseOrder(SqlConnection connection, clsPurchaseOrder order);
    }
}