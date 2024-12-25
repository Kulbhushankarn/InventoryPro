using InventoryPro.DL.ITF;
using InventoryPro.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryPro.BM
{
    public class clsNotificationBM
    {
        private readonly IclsNotificationDL _notificationDL;

        public clsNotificationBM(IclsNotificationDL notificationDL)
        {
            _notificationDL = notificationDL;
        }

        public List<clsNotification> GetAllNotifications()
        {
            return _notificationDL.GetAllNotifications();
        }

        public void AddNotification(clsNotification notification)
        {
            _notificationDL.AddNotification(notification);
        }
    }
}
