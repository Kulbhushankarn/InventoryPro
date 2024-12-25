using InventoryPro.VO;
using System.Collections.Generic;

namespace InventoryPro.DL.ITF
{
    public interface IclsNotificationDL
    {
        void AddNotification(clsNotification notification);
        List<clsNotification> GetAllNotifications();
    }
}