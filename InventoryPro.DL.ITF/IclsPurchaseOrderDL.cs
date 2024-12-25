using InventoryPro.VO;
using System.Collections.Generic;

namespace InventoryPro.DL.ITF
{
    public interface IclsPurchaseOrderDL
    {
        void CreatePurchaseOrder(clsPurchaseOrder order);
        List<clsPurchaseOrder> GetAllPurchaseOrders();
    }
}