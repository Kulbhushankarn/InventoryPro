using InventoryPro.VO;
using System.Collections.Generic;

namespace InventoryPro.BM.ITF
{
    public interface IclsSalesRecordBM
    {
        void AddSalesRecord(clsSalesRecord record);
        List<clsSalesRecord> GetAllSalesRecords();
    }
}