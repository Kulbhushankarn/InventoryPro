using InventoryPro.VO;
using System.Collections.Generic;

namespace InventoryPro.DL.ITF
{
    public interface IclsSalesRecordDL
    {
        void AddSalesRecord(clsSalesRecord record);
        List<clsSalesRecord> GetAllSalesRecords();
    }
}