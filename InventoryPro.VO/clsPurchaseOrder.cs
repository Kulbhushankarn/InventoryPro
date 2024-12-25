using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryPro.VO
{
    public class clsPurchaseOrder
    {
        public int OrderId { get; set; }
        public int ItemId { get; set; }
        public string SupplierName { get; set; }
        public DateTime OrderDate { get; set; }
        public int Quantity { get; set; }
        public decimal TotalCost { get; set; }
        public string Status { get; set; }
    }
}
