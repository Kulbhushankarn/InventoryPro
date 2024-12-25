using InventoryPro.DL.ITF;
using InventoryPro.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryPro.BM
{
    public class clsSalesRecordBM
    {
        private readonly IclsSalesRecordDL _salesRecordDL;

        public clsSalesRecordBM(IclsSalesRecordDL salesRecordDL)
        {
            _salesRecordDL = salesRecordDL;
        }

        public List<clsSalesRecord> GetAllSalesRecords()
        {
            return _salesRecordDL.GetAllSalesRecords();
        }

        public void AddSalesRecord(clsSalesRecord record)
        {
            _salesRecordDL.AddSalesRecord(record);
        }
    }
}
