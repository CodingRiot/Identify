using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace identify.Notification
{
    public class Event
    {
        //public EventType TypeOfEvent { get; set; }
        public string Details { get; set; }
        public DateTime TimeOccured { get; set; }

        public enum EventType
        {
            Browser,
            File,
            Application,
            Mouse,
            Keyboard,
            ActiveApplication,
            ERROR
        }


    }
}
