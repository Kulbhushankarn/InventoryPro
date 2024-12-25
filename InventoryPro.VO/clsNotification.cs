using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryPro.VO
{
    public class clsNotification
    {
        public int NotificationId { get; set; }
        public string Message { get; set; }
        public string Type { get; set; } // E.g., Info, Warning, Error
        public bool IsRead { get; set; }
        public int UserId { get; set; }
    }
}
