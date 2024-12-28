using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using InventoryPro.BM.ITF;
using InventoryPro.DL.ITF;
using InventoryPro.VO;

namespace InventoryPro.BM
{
    public class clsPurchaseOrderBM : IclsPurchaseOrderBM
    {
        private readonly IclsPurchaseOrderDL _purchaseOrderDL;

        public clsPurchaseOrderBM(IclsPurchaseOrderDL purchaseOrderDL)
        {
            _purchaseOrderDL = purchaseOrderDL;
        }

        public List<clsPurchaseOrder> GetAllPurchaseOrders(SqlConnection connection)
        {
            return _purchaseOrderDL.GetAllPurchaseOrders(connection);
        }

        public clsPurchaseOrder GetPurchaseOrderById(SqlConnection connection, int id)
        {
            return _purchaseOrderDL.GetPurchaseOrderById(connection, id);
        }

        public void CreatePurchaseOrder(SqlConnection connection, clsPurchaseOrder order)
        {
            if (order.TotalCost <= 0)
            {
                throw new ArgumentException("Total amount must be greater than zero.");
            }

            _purchaseOrderDL.CreatePurchaseOrder(connection, order);
        }

        public void UpdatePurchaseOrder(SqlConnection connection, clsPurchaseOrder order)
        {
            if (order.TotalCost <= 0)
            {
                throw new ArgumentException("Total amount must be greater than zero.");
            }

            _purchaseOrderDL.UpdatePurchaseOrder(connection, order);
        }

        public void DeletePurchaseOrder(SqlConnection connection, int id)
        {
            _purchaseOrderDL.DeletePurchaseOrder(connection, id);
        }

        public List<clsPurchaseOrder> GetOrdersAboveThreshold(SqlConnection connection, decimal threshold)
        {
            return _purchaseOrderDL.GetOrdersAboveThreshold(connection, threshold);
        }
    }
}
