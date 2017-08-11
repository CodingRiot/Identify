using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using identify.Logs;
using identify.Notification;

namespace identify.ActiveApp
{
    
    class ActiveAppChecker
    {
        public ActiveApplication ActiveApp = new ActiveApplication();
        public ActiveAppHook ActiveApplicationHook = new ActiveAppHook();
        public ActiveAppChecker()
        {
            var activeAppTimer = new System.Timers.Timer(1000);
            activeAppTimer.Elapsed += new ElapsedEventHandler(OnActiveAppCheck);
            activeAppTimer.Enabled = true;
        }

        private void OnActiveAppCheck(object sender, ElapsedEventArgs e)
        {
            ActiveApp.ApplicationName = ActiveApplicationHook.GetForegroundProcessName();

            if (ActiveApp.ApplicationName != ActiveApp.PreviouslyAccessedAppName)
            {
                Logging.LogEntry(Event.EventType.ActiveApplication, ActiveApp.ApplicationName, null);
            }

            ActiveApp.PreviouslyAccessedAppName = ActiveApp.ApplicationName;
        }
    }
}
