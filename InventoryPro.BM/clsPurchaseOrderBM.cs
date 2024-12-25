using InventoryPro.DL.ITF;
using InventoryPro.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryPro.BM
{
    public class clsPurchaseOrderBM
    {
        private readonly IclsPurchaseOrderDL _purchaseOrderDL;

        public clsPurchaseOrderBM(IclsPurchaseOrderDL purchaseOrderDL)
        {
            _purchaseOrderDL = purchaseOrderDL;
        }

        public List<clsPurchaseOrder> GetAllPurchaseOrders()
        {
            return _purchaseOrderDL.GetAllPurchaseOrders();
        }

        public void CreatePurchaseOrder(clsPurchaseOrder order)
        {
            _purchaseOrderDL.CreatePurchaseOrder(order);
        }
    }
}
