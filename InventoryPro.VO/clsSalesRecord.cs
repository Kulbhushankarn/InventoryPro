using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryPro.VO
{
    public class clsSalesRecord
    {
        public int SaleId { get; set; }
        public int ItemId { get; set; }
        public DateTime SaleDate { get; set; }
        public int QuantitySold { get; set; }
        public decimal SaleAmount { get; set; }
    }
}