using System;
using System.Timers;
using identify.Logs;

namespace identify.Mouse
{
    class CheckMouseStatus
    {
        public static int MouseMovementsCount { get; set; }

        public CheckMouseStatus()
        {
            var mouseTimer = new System.Timers.Timer(60000);
            mouseTimer.Elapsed += new ElapsedEventHandler(OnMouseCheck);
            mouseTimer.Enabled = true;
        }

        private void OnMouseCheck(object sender, ElapsedEventArgs e)
        {
            if (MouseMovementsCount >= 1)
            {
                Logging.LogEntry(Notification.Event.EventType.Mouse, String.Format("Moved: {0}", MouseMovementsCount));
            }
            
            MouseMovementsCount = 0;  // Reset Mouse counter
        }
    }
}
